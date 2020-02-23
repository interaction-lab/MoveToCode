using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MoveToCode {
    public class Exercise : MonoBehaviour {
        public string consoleStringGoal;

        public bool IsExerciseCorrect() {
            Debug.Log(ConsoleManager.instance.GetCleanedMainText());
            Debug.Log(consoleStringGoal);
            return ConsoleManager.instance.GetCleanedMainText() == consoleStringGoal;
        }
    }
}
