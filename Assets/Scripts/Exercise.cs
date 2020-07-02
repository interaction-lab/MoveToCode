using UnityEngine;
using UnityEngine.Assertions;

namespace MoveToCode {
    //[RequireComponent(typeof(ExerciseInformationSeekingActions))]
    //[RequireComponent(typeof(ExerciseScaffolding))]
    public class Exercise : MonoBehaviour {
        ExerciseInternals myExerciseInternals;

        public Exercise() { }

        public void SetExerciseInternals(string address) {
            myExerciseInternals = JsonUtility.FromJson<ExerciseInternals>(address);
        }

        public ExerciseInternals GetExerciseInternals() {
            return myExerciseInternals;
        }

        protected virtual void OnEnable() {
        }

    }
}
