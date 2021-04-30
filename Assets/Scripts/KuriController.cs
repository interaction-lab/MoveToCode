using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {

    /// <summary>
    /// Base class that controls Kuri's Behaviors
    /// </summary>
    public abstract class KuriController : MonoBehaviour {

        public string TakeISAAction() {
            string actionString = ExerciseManager.instance.GetCurExercise().GetComponent<ExerciseInformationSeekingActions>().DoISAAction();
            return actionString;
        }

        public abstract void TakeMovementAction();

        public abstract void DoPositiveAffect(KuriTextManager.TYPEOFAFFECT toa);


    }
}
