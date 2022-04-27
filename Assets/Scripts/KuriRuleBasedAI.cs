using UnityEngine;

namespace MoveToCode {
    public class KuriRuleBasedAI : KuriAI {
        #region members
        TutorKuriManager kuriManager;
        KuriController kuriController;
        int ISACount = 0;
        int movementActionCount = 0;
        #endregion
        #region unity
        private void Awake() {
            kuriManager = TutorKuriManager.instance;
            kuriController = kuriManager.kuriController;
        }
        #endregion
        #region public
        public override void Tick() {
            if (kuriManager.TimeLastActionStarted.TimeSince() < kuriManager.TimeWindow) {
                return;
            }
            float kctS = HumanStateManager.instance.GetKCt();
            if (kctS < kuriManager.robotKC) {
                TakeKCAction();
            }
            else {
                kuriController.DoRandomPositiveAction();
            }
        }
        #endregion
        #region private
        private void TakeKCAction() {
            if (movementActionCount == 0 && ISACount == 0) {
                int rand = Random.Range(0, 2);
                if (rand == 0) {
                    TakeISAAction();
                }
                else {
                    TakeMovementAction();
                }
            }
            else if (movementActionCount == 0) {
                TakeMovementAction();
            }
            else if (ISACount == 0) {
                TakeISAAction();
            }
            else {
                // take random action proportional to ISACount and movementActionCount
                int rand = Random.Range(0, movementActionCount + ISACount);
                if (rand < movementActionCount) {
                    TakeMovementAction();
                }
                else {
                    TakeISAAction();
                }
            }
        }

        private void TakeMovementAction() {
            kuriController.TakeMovementAction();
            ++movementActionCount;
        }

        private void TakeISAAction() {
            kuriController.TakeISAAction();
            ++ISACount;
        }
        #endregion
    }
}
