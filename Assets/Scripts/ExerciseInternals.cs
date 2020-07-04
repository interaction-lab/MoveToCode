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


        public string GetConsoleStringGoal() {
            return consoleStringGoal;
        }

        public string GetKuriGoalString() {
            return kuriGoalString;
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
    }
}
