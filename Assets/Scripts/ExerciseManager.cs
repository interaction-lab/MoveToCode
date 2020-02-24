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
                ex.enabled = false;
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
            ToggleCurrentExercise(false);
            curExercisePos += 1 % exerciseList.Count; // TODO: add a free play at the end
            curExercise = exerciseList[curExercisePos];
            ToggleCurrentExercise(true);
        }

        private void ToggleCurrentExercise(bool desiredActiveState) {
            if (!desiredActiveState) {
                curExercise.enabled = false;
            }
            curExercise.gameObject.SetActive(desiredActiveState);
            if (desiredActiveState) {
                curExercise.enabled = true;
            }
        }
    }
}
