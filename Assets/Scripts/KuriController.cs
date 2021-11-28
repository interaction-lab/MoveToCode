﻿using UnityEngine;

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
        public string CurAction = "";
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

        protected KuriTextManager kuriTextManager;
        protected LoggingManager loggingManager;
        private void Awake() {
            loggingManager = LoggingManager.instance;
            kuriTextManager = KuriTextManager.instance;
            loggingManager.AddLogColumn(rISACol, "");
            loggingManager.AddLogColumn(kuriPhysicalEmoteActionCol, "");
            loggingManager.AddLogColumn(kuriMovementActionCol, "");
        }
        public string TakeISAAction() {
            string actionString = ExerciseManager.instance.GetCurExercise().GetComponent<ExerciseInformationSeekingActions>().DoISAAction();
            loggingManager.UpdateLogColumn(rISACol, actionString);
            return actionString;
        }

        public void SayExerciseGoal() {
            kuriTextManager.Addline(string.Join("",
                 "Goal: ",
                 ExerciseManager.instance.GetCurExercise().GetGoalString()),
                 KuriTextManager.PRIORITY.high);
        }

        public void SayAndDoPositiveAffect(KuriTextManager.TYPEOFAFFECT toa) {
            TurnTowardsUser();
            string actionMade = DoRandomPositiveAction();
            loggingManager.UpdateLogColumn(kuriPhysicalEmoteActionCol,
                 actionMade);

            kuriTextManager.Clear(KuriTextManager.PRIORITY.low);
            kuriTextManager.SayRandomPositiveAffect(toa);
        }

        public void DoScaffoldingDialogue() {
            TurnTowardsUser();
            ExerciseManager.instance.GetCurExercise().GetComponent<ExerciseScaffolding>().SayNextScaffold();
        }

        private void Update() {
            IsDoingAction = KuriTextManager.instance.IsTalking;
        }

        public abstract string TakeMovementAction();
        public abstract string DoRandomPositiveAction();
        public abstract string DoRandomNegativeAction();
        public abstract string DoAction(EMOTIONS e);
        public abstract void TurnTowardsUser();
    }
}
