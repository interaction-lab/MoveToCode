using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class KuriAIBTRandom : KuriAI {
        #region members
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
        #endregion

        #region unity
        #endregion

        #region public
        public override void ForceHelpfulAction() {
            throw new System.NotImplementedException();
        }

        public override void Tick() {
            // launch event every 15 seconds
            Debug.Log(TutorKuriManagerInstance.TimeLastActionEnded.TimeSince());
            if (TutorKuriManagerInstance.TimeLastActionEnded.TimeSince() > 15) {
                DoRandomBTAction();
            }
            else {
                // randomly call do look_around Emotion
                // check if animating anything right now
                if (BodyAnimator.IsFullyIdle() && (Random.Range(0, 1000) < 1)) {
                    BodyAnimator.Play(KuriController.EMOTIONS.look_around.ToString());
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
                KController.PointAtObj(objOfInterest);
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
        #endregion
    }
}
