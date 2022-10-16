using System;
using UnityEngine;

namespace MoveToCode {
    public class KuriAIBTRandom : KuriAI {
        #region members
        // TODO:
        // 1. add in logger for these actions
        // 2. add in helpful action for code blocks
        // 2a. possibly figure out what is wrong about the solution
        // 2b. say how to fix
        // 3. add in helpful action for maze
        // 3a. figure out what is wrong with solution
        // 4. identify if the user is just lost af
        // 5. make another idling behavior
        SwitchModeButton smb;
        SwitchModeButton ModeButton {
            get {
                if (smb == null) {
                    smb = SwitchModeButton.instance;
                }
                return smb;
            }
        }
        MazeManager mm;
        MazeManager MazeManagerInstance {
            get {
                if (mm == null) {
                    mm = MazeManager.instance;
                }
                return mm;
            }
        }
        KuriBTBodyController kc;
        KuriBTBodyController KController {
            get {
                if (kc == null) {
                    kc = GetComponent<KuriBTBodyController>();
                }
                return kc;
            }
        }
        TutorKuriManager tkm;
        TutorKuriManager TutorKuriManagerInstance {
            get {
                if (tkm == null) {
                    tkm = TutorKuriManager.instance;
                }
                return tkm;
            }
        }
        Animator _bodyAnimator;
        public Animator BodyAnimator {
            get {
                if (_bodyAnimator == null) {
                    _bodyAnimator = GetComponent<Animator>();
                }
                return _bodyAnimator;
            }
        }
        Animator _armAnimator;
        public Animator ArmAnimator {
            get {
                if (_armAnimator == null) {
                    _armAnimator = KuriArms.instance.ArmAnimator;
                }
                return _armAnimator;
            }
        }
        Interpreter interpreter;
        Interpreter InterpreterInstance {
            get {
                if (interpreter == null) {
                    interpreter = Interpreter.instance;
                }
                return interpreter;
            }
        }
        // need to maze completed action

        bool initialized = false;
        #endregion

        #region unity
        private void OnEnable() {
            if (!initialized) {
                ModeButton.OnSwitchToMazeBuildingMode.AddListener(OnSwitchToBuildingMode);
                ModeButton.OnSwitchToCodingMode.AddListener(OnSwitchToCodingMode);
                ExerciseManager.instance.OnExerciseCorrect.AddListener(OnExerciseCorrect);
                initialized = true;
            }
        }
        #endregion

        #region public
        public override void ForceHelpfulAction() {
            _DoHelpfulActionGivenMode();
        }


        public override void Wave() {
            KController.DoAnimationAction(KuriController.EMOTIONS.NewWave);
        }
        private bool KuriIdling() {
            return BodyAnimator.IsInAnIdleState() &&
                ArmAnimator.IsInAnIdleState();
        }

        public override void Tick() {
            if (!KuriIdling() ||
                TutorKuriManagerInstance.TimeLastActionStarted > TutorKuriManagerInstance.TimeLastActionEnded ||
                TutorKuriManagerInstance.TimeLastActionEnded.TimeSince() < TutorKuriManagerInstance.TimeWindow ||
                InterpreterInstance.CodeIsRunning()) {
                return;
            }

            float kctS = HumanStateManager.instance.GetKCt(); // encourage curiosity when they are doing low amounts of KCT
            if (kctS < TutorKuriManagerInstance.robotKC) {
                ForceHelpfulAction();
            }
            else {
                if (HumanStateManager.instance.LastTimeHumanDidAction.TimeSince() >= TutorKuriManagerInstance.TimeWindow) { // only encrourage if the human isn't doing anything
                    DoRandomBTAction();
                }
                else {
                    // randomly call do look_around Emotion
                    // check if animating anything right now
                    if (BodyAnimator.IsFullyIdle() &&
                         (UnityEngine.Random.Range(0, 1000) < 1)) {
                        // set head quaternion to identity
                        BodyAnimator.Play(KuriController.EMOTIONS.look_around.ToString()); // need one more behavior for this, maybe just some blinking or something?
                    }
                }
            }

        }

        public void DoRandomBTAction() {
            int rand = UnityEngine.Random.Range(2, 5);

            if (rand == 2) {
                KController.TurnTowardsUser();
            }
            else if (rand == 3) {
                // random happy
                KController.DoAnimationAction(KuriController.PositiveEmotions[UnityEngine.Random.Range(0, KuriController.PositiveEmotions.Length)]);
            }
            else if (rand == 4) {
                // random sad
                KController.DoAnimationAction(KuriController.NegativeEmotions[UnityEngine.Random.Range(0, KuriController.NegativeEmotions.Length)]);
            }
        }
        #endregion

        #region private
        void OnSwitchToBuildingMode() {
            OnSwitchMode();
        }
        void OnSwitchToCodingMode() {
            OnSwitchMode();
        }

        void OnSwitchMode() {
            KuriTextManager.instance.Clear(KuriTextManager.PRIORITY.low);
            KController.StopAllBeh();
        }
        void _DoHelpfulActionGivenMode() {
            if (ModeButton.CurrentMode == SwitchModeButton.MODE.MazeBuilding) {
                _DoMazeBuildingHelpfulAction();
            }
            else {
                _DoCodingHelpfulAction();
            }
        }

        // need to do something on exercise correct as well :)

        void _DoMazeBuildingHelpfulAction() {
            if (MazeManagerInstance.IsSameAsSolutionMaze()) {
                KController.DoRandomPositiveAction();
                return;
            }

            // now we know the maze is not correct, look for the piece to give someone
            MazePiece mp = MazeManagerInstance.GetMissingPiecesFromMaze();
            bool missingPiece = mp != null;
            if (!missingPiece) {
                mp = MazeManagerInstance.GetMisalignedPiece(); // need to get a misaligned as maze has all the same pieces as sol
                if (mp == null) { // this happens when the maze has extra pieces
                    KuriTextManager.instance.Addline("Too many pieces in maze, make sure it looks exactly like the solution maze.");
                    KController.DoRandomPositiveAction();
                    return;
                }
                KuriTextManager.instance.Addline(
                    "It looks like your " +
                    mp.MyMPType.Name + " " +
                    "piece is not connected right. Move the piece around to connect it to look exactly like the solution maze.",
                    KuriTextManager.PRIORITY.low);
            }
            else {
                KuriTextManager.instance.Addline(
                    "It looks like your maze is missing a " +
                    mp.MyMPType.Name + " " +
                    "piece.",
                    KuriTextManager.PRIORITY.low);
            }

            if (!mp.HasBeenTracked) {
                KController.PointToPaper(mp.gameObject.name);
            }
            else {
                if (UnityEngine.Random.Range(0, 2) == 0) {
                    KController.PointToPaper(mp.gameObject.name);
                }
                else {
                    KController.OnlyLookAtObj(mp.transform);
                    KController.MoveToThenPoint(mp.transform);
                }
            }
        }

        private void _DoCodingHelpfulAction() {
            if (!MazeManagerInstance.IsSameAsSolutionMaze()) {
                KuriTextManager.instance.Addline("I don't think the mazes match, press the 'Switch Mode' to build the maze.");
                return;
            }

            // first = said something, second = objofinterest
            Pair<bool, Transform> pair = ExerciseManager.instance.SayScaffoldingOfCurExercise(); // this needs to also return an obj to move to

            if (pair.First && pair.Second != null) {
                Transform blockTransformOfInterest = pair.Second;
                ArrowPointPrefab vpArrowPoint = ViewPortManager.instance.GetArrowPoint(blockTransformOfInterest);
                if (vpArrowPoint == null) {
                    KController.PointAtObj(blockTransformOfInterest); // this will spawn the arrow point for later/first time using it
                }
                KController.MoveToThenPoint(blockTransformOfInterest);
            }
            else if (!pair.First) { // used up all scaffolding
                KController.SayAndDoPositiveAffect(KuriTextManager.TYPEOFAFFECT.Encouragement);
            }
        }

        void OnExerciseCorrect() {
            KController.SayAndDoPositiveAffect(KuriTextManager.TYPEOFAFFECT.Congratulation);
        }
        #endregion
    }
}
