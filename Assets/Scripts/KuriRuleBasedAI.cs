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
            kuriController = kuriManager.KController;
            SolMazeCheckMark.instance.OnMazeCorrect.AddListener(OnMazeCorrect);
            MazeManager.instance.OnMazeLocked.AddListener(OnMazeLocked);
        }
        #endregion
        #region public
        public override void Tick() {
            if (kuriController.IsDoingAction || kuriManager.TimeLastActionEnded.TimeSince() < kuriManager.TimeWindow) {
                return;
            }
            float kctS = HumanStateManager.instance.GetKCt(); // encourage curiosity when they are doing low amounts of KCT
            if (kctS < kuriManager.robotKC) {
                TakeKCAction();
            }
            else {
                if (HumanStateManager.instance.LastTimeHumanDidAction.TimeSince() >= kuriManager.TimeWindow) { // only encrourage if the human isn't doing anything
                    kuriController.SayAndDoPositiveAffect(KuriTextManager.TYPEOFAFFECT.Encouragement);
                }
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
            // if maze isn't built, move to specific maze piece
            if (!MazeManager.instance.ContainsSolutionMaze()) {
                kuriController.TakeMovementAction(3);
            }
            // if maze is built, try scaffolding
            else {
                ExerciseManager.instance.GetCurExercise().GetComponent<ExerciseScaffolding>().SayNextScaffold();
            }
            ++ISACount;
        }

        public override void ForceHelpfulAction() {
            TakeISAAction();
        }

        bool saidSwitchToCoding = false;
        void OnMazeCorrect() {
            if (ExerciseManager.instance.GetCurExercise().SaidMazeGoal && !saidSwitchToCoding) {
                KuriTextManager.instance.Clear(KuriTextManager.PRIORITY.high);
                kuriController.SayAndDoPositiveAffect(KuriTextManager.TYPEOFAFFECT.Congratulation);
                saidSwitchToCoding = true;
            }
        }

        void OnMazeLocked() {
            if (!ExerciseManager.instance.GetCurExercise().SaidCodingGoal) {
                KuriTextManager.instance.Clear(KuriTextManager.PRIORITY.high);
                kuriController.SayCodeGoal();
            }
        }
        #endregion
    }
}
