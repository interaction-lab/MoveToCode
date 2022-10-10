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
        List<Transform> _objsOfInterest = null;
        List<Transform> objsOfInterest {
            get {
                if (_objsOfInterest == null) {
                    _objsOfInterest = new List<Transform>(){
                            StartCodeBlock.instance.transform,
                            Camera.main.transform,
                            MazeManager.instance.BKMazePiece.transform,
                            MazeManager.instance.GoalMazePiece.transform
                    };
                }
                return _objsOfInterest;
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
        List<CodeBlock> ActiveCBs {
            get {
                return CodeBlockManager.instance.ActiveCodeBlocks;
            }
        }
        #endregion

        #region unity
        #endregion

        #region public
        public override void ForceHelpfulAction() {
            _DoHelpfulActionGivenMode();
        }

        public override void Tick() {
            // launch event every 15 seconds
            Debug.Log(TutorKuriManagerInstance.TimeLastActionEnded.TimeSince());
            if (TutorKuriManagerInstance.TimeLastActionEnded.TimeSince() > 15) {
                TutorKuriManagerInstance.TimeLastActionEnded = Time.time;
                DoRandomBTAction();
            }
            else {
                // randomly call do look_around Emotion
                // check if animating anything right now
                if (BodyAnimator.IsFullyIdle() && (Random.Range(0, 1000) < 1)) {
                    // set head quaternion to identity
                    BodyAnimator.Play(KuriController.EMOTIONS.look_around.ToString()); // need one more behavior for this, maybe just some blinking or something?
                }
            }

        }

        public void DoRandomBTAction() {

            // choose random object from objsOfInterest
            Transform objOfInterest = objsOfInterest[Random.Range(0, objsOfInterest.Count)];

            // pikc a random number 0 or 1
            int rand = Random.Range(0, 5);
            Debug.Log(rand);

            if (rand == 0) {
                KController.MoveToObj(objOfInterest);
            }
            else if (rand == 1) {
                KController.PointUntilInteract(objOfInterest);
            }
            else if (rand == 2) {
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
        void _DoHelpfulActionGivenMode() {
            if (ModeButton.CurrentMode == SwitchModeButton.MODE.MazeBuilding) {
                _DoMazeBuildingHelpfulAction();
            }
            else {
                //_DoCodingHelpfulAction();
            }
        }

        void _DoMazeBuildingHelpfulAction() {
            if (MazeManagerInstance.IsSameAsSolutionMaze()) {
                KController.DoRandomPositiveAction();
                return;
            }

            // now we know the maze is not correct, look for the piece to give someone
            MazePiece mp = MazeManagerInstance.GetMissingPiecesFromMaze();
            if (mp == null) {
                mp = MazeManagerInstance.GetMisalignedPiece(); // need to get a misaligned as maze has all the same pieces as sol
            }

            if (!mp.HasBeenTracked) {
                // hold up picture until it has been tracked -> TODO: implement this
            }
            else {
                KController.PointAtObj(mp.transform);
                KController.MoveToObj(mp.transform);
            }
        }
        #endregion
    }
}
