using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace MoveToCode {
    public class ExerciseManager : Singleton<ExerciseManager> {
        public static string curExcersieCol = "CurExercise", exerciseSubmissionResultCol = "ExerciseSubmissionResult";
        public UnityEvent OnExerciseCorrect, OnCyleNewExercise;
        Exercise curExercise;
        List<Exercise> exerciseList;
        public int curExercisePos = 0;
        bool lastExerciseCompleted = false;

        private void Awake() {
#if !UNITY_EDITOR
            curExercisePos = 0;
#endif
            SetUpExerciseList();
            curExercise = exerciseList[curExercisePos];
            ToggleCurrentExercise(true);
            LoggingManager.instance.AddLogColumn(curExcersieCol, curExercisePos.ToString());
            LoggingManager.instance.AddLogColumn(exerciseSubmissionResultCol, "");
        }

        private void SetUpExerciseList() {
            exerciseList = new List<Exercise>();
            foreach (Exercise ex in GetComponentsInChildren<Exercise>(true)) {
                exerciseList.Add(ex);
                ex.gameObject.SetActive(false);
            }
        }

        public Exercise GetCurExercise() {
            return curExercise;
        }

        public Pair<bool, Transform> SayScaffoldingOfCurExercise() {
            return curExercise.GetComponent<ExerciseScaffolding>().SayNextScaffold();
        }

        public bool AlertCodeFinished() {
            if (curExercise != null) { // This if is to guard against initializing interpreter
                if (MazeManager.instance.IsBKAtTheGoalNow() && MazeManager.instance.IsSameAsSolutionMaze()) {
                    LoggingManager.instance.UpdateLogColumn(exerciseSubmissionResultCol, "Correct");
                    lastExerciseCompleted = true;
                    OnExerciseCorrect.Invoke();
                    return true;
                }
                else {
                    TutorKuriManager.instance.KController.TriggerHelpfulAction();
                    LoggingManager.instance.UpdateLogColumn(exerciseSubmissionResultCol, "InCorrect");
                    return false;
                }
            }
            return true;
        }

        public void AlertCodeReset() {
            if (lastExerciseCompleted) {
                CycleNewExercise();
            }
        }

        private void CycleNewExercise() {
            lastExerciseCompleted = false;
            curExercise.CleanUp();
            ToggleCurrentExercise(false);
            OnCyleNewExercise.Invoke();
            curExercisePos += 1;
            LoggingManager.instance.UpdateLogColumn(curExcersieCol, curExercisePos.ToString());
            curExercise = exerciseList[curExercisePos];
            ToggleCurrentExercise(true);
        }

        private void ToggleCurrentExercise(bool desiredActiveState) {
            if (curExercise as FreePlayExercise != null) {
                CodeBlockMenuManager.instance.TurnMenuOn();
            }
            else {
                CodeBlockMenuManager.instance.TurnMenuOff();
            }
            curExercise.gameObject.SetActive(desiredActiveState);
            SolMazeManager.instance.LogMaze();
        }
    }
}