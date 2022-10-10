using System.Collections.Generic;
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
        Animator BodyAnimator {
            get {
                if (_bodyAnimator == null) {
                    _bodyAnimator = GetComponent<Animator>();
                }
                return _bodyAnimator;
            }
        }
        Animator _armAnimator;
        Animator ArmAnimator {
            get {
                if (_armAnimator == null) {
                    _armAnimator = KuriArms.instance.ArmAnimator;
                }
                return _armAnimator;
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

        private bool KuriIdling() {
            return BodyAnimator.IsInAnIdleState() &&
                ArmAnimator.IsInAnIdleState();
        }

        public override void Tick() {
            if (!KuriIdling() || TutorKuriManagerInstance.TimeLastActionEnded.TimeSince() < TutorKuriManagerInstance.TimeWindow) {
                return;
            }

            float kctS = HumanStateManager.instance.GetKCt(); // encourage curiosity when they are doing low amounts of KCT
            if (kctS < TutorKuriManagerInstance.robotKC) {
                TutorKuriManagerInstance.TimeLastActionEnded = Time.time;
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
                         (Random.Range(0, 1000) < 1)) {
                        // set head quaternion to identity
                        BodyAnimator.Play(KuriController.EMOTIONS.look_around.ToString()); // need one more behavior for this, maybe just some blinking or something?
                    }
                }
            }

        }

        public void DoRandomBTAction() {
            int rand = Random.Range(2, 5);

            if (rand == 2) {
                KController.TurnTowardsUser();
            }
            else if (rand == 3) {
                // random happy
                KController.DoAnimationAction(KuriController.PositiveEmotions[Random.Range(0, KuriController.PositiveEmotions.Length)]);
            }
            else if (rand == 4) {
                // random sad
                KController.DoAnimationAction(KuriController.NegativeEmotions[Random.Range(0, KuriController.NegativeEmotions.Length)]);
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
                KuriTextManager.instance.Addline(
                    "It looks like your " +
                    mp.MyMPType.Name + " " +
                    "piece is not connected right. Move the piece around to connect it to look exactlly like the solution maze.",
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
                // hold up picture until it has been tracked -> TODO: implement this
            }
            else {
                KController.PointAtObj(mp.transform);
                KController.MoveToObj(mp.transform);
            }
        }

        private void _DoCodingHelpfulAction() {
            if (!MazeManagerInstance.IsSameAsSolutionMaze()) {
                KuriTextManager.instance.Addline("I don't think the mazes match, press the 'Switch Mode' to build the maze.");
                return;
            }

            // these really have to be exercise specific, maybe hardcoding in a bunch of help conditions although that will be very difficult, I think we might be able to just rely on the idea that students will hopefully be curious during this stage and we can just leave them alone/possibly animate randomly
            // check if start code block is out of view
            Transform blockTransformOfInterest = StartCodeBlock.instance.transform; // should probably turn this into a function to use any block of interest
            ArrowPointPrefab startArrowPoint = ViewPortManager.instance.GetArrowPoint(blockTransformOfInterest);
            if (startArrowPoint == null) {
                KController.PointAtObj(blockTransformOfInterest); // this will spawn the arrow point for later/first time using it
                KController.MoveToObj(blockTransformOfInterest);
            }
            else if (!startArrowPoint.IsInViewPort) {
                KController.PointAtObj(blockTransformOfInterest);
                KController.MoveToObj(blockTransformOfInterest);
            }
        }

        void OnExerciseCorrect() {
            KController.SayAndDoPositiveAffect(KuriTextManager.TYPEOFAFFECT.Congratulation);
        }
        #endregion
    }
}
