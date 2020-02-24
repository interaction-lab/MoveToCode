using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ExerciseManager : Singleton<ExerciseManager> {
        Exercise curExercise;
        List<Exercise> exerciseList;
        int curExercisePos = 0;
        bool lastExerciseCompleted = false;

        private void Awake() {
            SetUpExerciseList();
            curExercise = exerciseList[curExercisePos];
            ToggleCurrentExercise(true);
        }

        private void SetUpExerciseList() {
            exerciseList = new List<Exercise>();
            foreach (Exercise ex in GetComponentsInChildren<Exercise>(true)) {
                exerciseList.Add(ex);
                ex.gameObject.SetActive(false);
            }
        }

        public void AlertCodeFinished() {
            if (curExercise != null && curExercise.IsExerciseCorrect()) {
                lastExerciseCompleted = true;
            }
        }

        public void AlertCodeReset() {
            if (lastExerciseCompleted) {
                CycleNewExercise();
            }
        }

        private void CycleNewExercise() {
            curExercise.UnsnapAllBlockFromBlockManager();
            ToggleCurrentExercise(false);
            curExercisePos += 1; // TODO: add a free play at the end
            if (curExercisePos == exerciseList.Count) {
                InitiateFreePlay();
            }
            else {
                curExercise = exerciseList[curExercisePos];
                ToggleCurrentExercise(true);
            }
        }

        private void InitiateFreePlay() {
            Debug.Log("Free play woould be initiated");
        }

        private void ToggleCurrentExercise(bool desiredActiveState) {
            curExercise.gameObject.SetActive(desiredActiveState);
        }
    }
}
