using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        public bool AlertCodeFinished() {
            if (curExercise != null) { // This if is to guard against initializing interpreter
                                       // curExercise.IsExerciseCorrect() -> old code
                if (MazeManager.instance.IsBKAtTheGoalNow() && MazeManager.instance.ContainsSolutionMaze()) {
                    TutorKuriManager.instance.kuriController.SayAndDoPositiveAffect(KuriTextManager.TYPEOFAFFECT.Congratulation);
                    LoggingManager.instance.UpdateLogColumn(exerciseSubmissionResultCol, "Correct");
                    lastExerciseCompleted = true;
                    OnExerciseCorrect.Invoke();
                    // this is where I should do the explosion at the goal
                    return true;
                }
                else {
                    TutorKuriManager.instance.kuriController.TriggerHelpfulAction();
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
            curExercisePos += 1;

            LoggingManager.instance.UpdateLogColumn(curExcersieCol, curExercisePos.ToString());
            curExercise = exerciseList[curExercisePos];
            ToggleCurrentExercise(true);
            OnCyleNewExercise.Invoke();
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