using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ExerciseManager : Singleton<ExerciseManager> {
        Exercise curExercise;

        private void Start() {
            curExercise = GetComponent<Exercise>();
        }

        public void AlertCodeFinished() {
            Debug.Log(curExercise.IsExerciseCorrect());
        }
    }
}
