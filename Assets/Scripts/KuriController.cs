using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {

    /// <summary>
    /// Base class that controls Kuri's Behaviors
    /// </summary>
    public abstract class KuriController : MonoBehaviour {
        public enum EMOTIONS {
            happy,
            neutral,
            sad,
            sassy,
            confused,
            thinking,
            love,
            close_eyes
        }


        public string TakeISAAction() {
            string actionString = ExerciseManager.instance.GetCurExercise().GetComponent<ExerciseInformationSeekingActions>().DoISAAction();
            return actionString;
        }

        public abstract string TakeMovementAction();

        public abstract string DoPositiveAffect(KuriTextManager.TYPEOFAFFECT toa);

        public abstract void PubRandomPositive();

        public abstract string DoAction(EMOTIONS e);


    }
}
