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
        public int[] finalVariableGoalValues;
        public CodeBlock[] exerciseCodeBlocks;
    }
}
