using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace MoveToCode {
    //[RequireComponent(typeof(ExerciseInformationSeekingActions))]
    //[RequireComponent(typeof(ExerciseScaffolding))]

    public class ExerciseInternals {
        public string consoleStringGoal;
        public string kuriGoalString;
        public string[] varNames;
        public int[] initialVariableValues;
        public object[] finalVariableGoalValues;
        public Pair[] exerciseBlocks;
        public string[] scaffoldDialogue;

        public string GetConsoleStringGoal() {
            return consoleStringGoal;
        }

        public string GetKuriGoalString() {
            return kuriGoalString;
        }

        public void SetConsoleStringGoal(string goal) {
            consoleStringGoal = goal;
        }

        public void SetKuriGoalString(string goal) {
            kuriGoalString = goal;
        }

        public string[] GetVarNames() {
            return varNames;
        }

        public int[] GetInitialVariableValues() {
            return initialVariableValues;
        }

        public object[] GetFinalVariableGoalValues() {
            return finalVariableGoalValues;
        }

        public Pair[] GetExerciseBlocks() {
            return exerciseBlocks;
        }

        public string[] GetscaffoldDialogue() {
            return scaffoldDialogue;
        }
    }
}
