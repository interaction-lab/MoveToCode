using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {

    /// <summary>
    /// Base class that controls Kuri's Behaviors. 
    /// Both physical and virtual KuriControllers inherit/implement this interface
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
        static string rISACol = "robotISA", kuriPhysicalEmoteActionCol = "kuriPhysicalAction", kuriMovementActionCol = "kuriMovementAction";
        public bool IsDoingAction = false;
        public string currentAction = "";
        public static EMOTIONS[] PositiveEmotions =
            new EMOTIONS[] {
                EMOTIONS.happy,
                EMOTIONS.love,
                EMOTIONS.thinking
            };

        public static EMOTIONS[] NegativeEmotions =
            new EMOTIONS[] {
                EMOTIONS.sad,
                EMOTIONS.sassy,
                EMOTIONS.confused
            };

        private void Awake() {
            LoggingManager.instance.AddLogColumn(rISACol, "");
            LoggingManager.instance.AddLogColumn(kuriPhysicalEmoteActionCol, "");
            LoggingManager.instance.AddLogColumn(kuriMovementActionCol, "");
        }
        public string TakeISAAction() {
            string actionString = ExerciseManager.instance.GetCurExercise().GetComponent<ExerciseInformationSeekingActions>().DoISAAction();
            LoggingManager.instance.UpdateLogColumn(rISACol, actionString);
            return actionString;
        }

        public abstract string TakeMovementAction();
        public abstract string DoRandomPositiveAction();
        public abstract string DoRandomNegativeAction();
        public abstract string DoAction(EMOTIONS e);
        public abstract void TurnTowardsUser();
    }
}
