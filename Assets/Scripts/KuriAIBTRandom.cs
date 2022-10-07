using UnityEngine;

namespace MoveToCode {
    public class KuriAIBTRandom : KuriAI {
        #region members
        KuriController kc;
        KuriController KController {
            get {
                if (kc == null) {
                    kc = GetComponent<KuriController>();
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
        #endregion

        #region unity
        #endregion

        #region public
        public override void ForceHelpfulAction() {
            throw new System.NotImplementedException();
        }

        public override void Tick() {
            // launch event every 15 seconds
            if (Time.time - TutorKuriManagerInstance.TimeLastActionEnded.TimeSince() > 15) {
                //DoRandomBTAction();
            }
        }

        public void DoRandomBTAction() {
            int rand = Random.Range(0, 5);
            Debug.Log(rand);
            if (rand == 0) {
                KController.MoveToObj(Camera.main.transform);
            }
            else if (rand == 1) {
                KController.PointAtObj(Camera.main.transform);
            }
            else if (rand == 2) {
                KController.TurnTowardsUser();
            }
            else if (rand == 3) {
                KController.DoAnimationAction(KuriController.EMOTIONS.happy);
            }
            else if (rand == 4) {
                KController.DoAnimationAction(KuriController.EMOTIONS.sad);
            }
        }
        #endregion

        #region private
        #endregion
    }
}
