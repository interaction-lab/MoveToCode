using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ExerciseManager : Singleton<ExerciseManager> {

        public void AlertCodeFinished() {
            // Check if correct
            Debug.Log(ConsoleManager.instance.GetCleanedMainText());
        }
    }
}
