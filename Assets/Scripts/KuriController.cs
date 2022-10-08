using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

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
            close_eyes,
            h5_start,
            h5_end,
            clap
        }
        protected static string rISACol = "robotISA", kuriPhysicalEmoteActionCol = "kuriPhysicalAction", kuriMovementActionCol = "kuriMovementAction", kuriCurAction = "kuriCurAction";
        //[HideInInspector]
        public bool IsDoingAction { get; set; } = false;
        //[HideInInspector]
        public string CurAction = "";
        public string actionSeperator = "~||~"; // used for data later
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
        public enum ACTIONS {
            VirtualISA,
            Movement,
            Idle,
            PositiveAffect,
            Scaffolding
        }

        public Dictionary<ACTIONS, float> lastTimeDidAction = Enum
        .GetValues(typeof(ACTIONS)).Cast<ACTIONS>()
        .ToDictionary(t => t, t => 0f);

        KuriTextManager ktm;
        protected KuriTextManager kuriTextManager {
            get {
                if (ktm == null) {
                    ktm = KuriTextManager.instance;
                }
                return ktm;
            }
        }


        LoggingManager lm;
        protected LoggingManager loggingManager {
            get {
                if (lm == null) {
                    lm = LoggingManager.instance;
                }
                return lm;
            }
        }

        public TargetIKObject IkObjLeft, IkObjRight;

        private void Awake() {
            SetUpLogColumns();
            SetUpIKTargets();
            Init();
        }

        private void SetUpIKTargets() {
            foreach (TargetIKObject tik in FindObjectsOfType<TargetIKObject>()) {
                if (tik.IsRightArm) {
                    IkObjRight = tik;
                }
                else {
                    IkObjLeft = tik;
                }
            }
        }

        private void SetUpLogColumns() {
            loggingManager.AddLogColumn(rISACol, "");
            loggingManager.AddLogColumn(kuriPhysicalEmoteActionCol, "");
            loggingManager.AddLogColumn(kuriMovementActionCol, "");
            loggingManager.AddLogColumn(kuriCurAction, "");
        }

        protected abstract void Init();
        public abstract string TakeISAAction();

        public void SayMazeGoal() {
            kuriTextManager.Addline(string.Join("",
                  "Goal: ",
                  ExerciseManager.instance.GetCurExercise().GetMazeGoalString()),
                  KuriTextManager.PRIORITY.high);
        }

        public void SayCodeGoal() {
            kuriTextManager.Addline(string.Join("",
                  "Goal: ",
                  ExerciseManager.instance.GetCurExercise().GetCodeGoalString()),
                  KuriTextManager.PRIORITY.high);
        }

        public void SayAndDoPositiveAffect(KuriTextManager.TYPEOFAFFECT toa) {
            TurnTowardsUser();
            string actionMade = DoRandomPositiveAction();
            loggingManager.UpdateLogColumn(kuriPhysicalEmoteActionCol,
                 actionMade);
            kuriTextManager.SayRandomPositiveAffect(toa);
        }

        public void TriggerHelpfulAction() {
            // TurnTowardsUser();
            ExerciseManager.instance.GetCurExercise().GetComponent<ExerciseScaffolding>().SayNextScaffold(); // old from when used to add specicif scaffolding for each maze, needs more contextual information
        }

        private void Update() {
            IsDoingAction = UpdateCurrentActionString();
            loggingManager.UpdateLogColumn(kuriCurAction, CurAction);
        }

        public abstract string TakeMovementAction(int option = -1);
        public abstract string MoveToObj(Transform obj);
        public abstract string DoRandomPositiveAction();
        public abstract string DoRandomNegativeAction();
        public abstract string DoAnimationAction(EMOTIONS e);
        public abstract void TurnTowardsUser();
        public abstract string PointAtObj(Transform objectOfInterest, float time = -1);
        // Returns true if currently doing action
        protected abstract bool UpdateCurrentActionString();
    }
}
