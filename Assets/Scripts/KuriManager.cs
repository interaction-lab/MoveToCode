using RosSharp.RosBridgeClient;
using System.Collections;
using UnityEngine;
using static MoveToCode.KuriController;

namespace MoveToCode {
    public class KuriManager : Singleton<KuriManager> {
        [Range(-3.0f, 3.0f)]
        public float robotKC;
        [HideInInspector]
        public bool usePhysicalKuri = true;
        static string robotKCLevel = "robotKCLevel";

        PoseStampedPublisher poseStampPublisher;

        public float TimeLastActionStarted { get; set; } = 0;
        public float TimeLastActionEnded { get; set; } = 0;
        public float TimeWindow { get; set; }

        public Transform KuriGoalPoseTransform;

        KuriController kuriControllerBackingVar = null;
        public KuriController kuriController {
            get {
                if (kuriController == null) {
                    if (usePhysicalKuri) {
                        kuriControllerBackingVar = FindObjectOfType<PhysicalKuriController>().GetComponent<KuriController>();
                    }
                    else {
                        kuriControllerBackingVar = FindObjectOfType<VirtualKuriController>().GetComponent<KuriController>();
                    }
                }
                return kuriControllerBackingVar;
            }
        }

        KuriAI kuriAIBackingVar = null;
        KuriAI kuriAI {
            get {
                if (kuriAIBackingVar == null) {
                    if (kuriAIToUse == KuriAI.KURIAI.Utility) {
                        kuriAIBackingVar = FindObjectOfType<KuriUtilityAI>().GetComponent<KuriUtilityAI>();
                    }
                    else if (kuriAIToUse == KuriAI.KURIAI.RuleBased) {
                        kuriAIBackingVar = FindObjectOfType<KuriRuleBasedAI>().GetComponent<KuriUtilityAI>();
                    }
                }
                return kuriAIBackingVar;
            }
        }
        public KuriAI.KURIAI kuriAIToUse;

        bool inStartUp;
        bool wasKuriDoingActionLastTick;

        private void Awake() {
            OptionSelectionManager.instance.Init();
            TimeWindow = HumanStateManager.instance.timeWindow;
            wasKuriDoingActionLastTick = kuriController.IsDoingAction;
            LoggingManager.instance.AddLogColumn(robotKCLevel, "");
            KuriGoalPoseTransform = transform.GetChild(0); // TODO: this is awful
        }

        private void Start() {
            StartCoroutine(StartRoutine());
        }

        public void SetKC(float kcRIn) {
            robotKC = kcRIn;
            LoggingManager.instance.UpdateLogColumn(robotKCLevel, robotKC.ToString("F3"));
        }

        IEnumerator StartRoutine() {
            inStartUp = true;
            yield return null;
            yield return new WaitForSeconds(3);
            kuriController.DoAction(EMOTIONS.close_eyes);
            yield return new WaitForSeconds(InteractionManager.instance.MinToSeconds(InteractionManager.instance.warmUpTimeMinutes) - 3f);
            kuriController.DoAction(EMOTIONS.happy);
            inStartUp = false;
        }

        private void Update() {
            Tick();
        }

        public void AlertActionStarted() {
            TimeLastActionStarted = Time.time;
        }


        private void Tick() {
            if (inStartUp) {
                return;
            }
            kuriAI.Tick();
            if (!wasKuriDoingActionLastTick && kuriController.IsDoingAction) {
                TimeLastActionStarted = Time.time;
            }
            else if (wasKuriDoingActionLastTick && !kuriController.IsDoingAction) {
                TimeLastActionEnded = Time.time;
            }
            UpdateEndOfTickVariables();
        }

        void UpdateEndOfTickVariables() {
            wasKuriDoingActionLastTick = kuriController.IsDoingAction;
        }

        public void TakeMovementAction() {
            KuriGoalPoseTransform.position = ExerciseInformationSeekingActions.goOfFocus.transform.position;
            KuriGoalPoseTransform.rotation = Quaternion.LookRotation(ExerciseInformationSeekingActions.goOfFocus.transform.forward);
            string actionMade = kuriController.TakeMovementAction();
            //LoggingManager.instance.UpdateLogColumn(kuriMovementActionCol, actionMade);
        }

        public void SayAndDoPositiveAffect(KuriTextManager.TYPEOFAFFECT toa) {
            kuriController.TurnTowardsUser();
            string actionMade = kuriController.DoRandomPositiveAction();
           // LoggingManager.instance.UpdateLogColumn(kuriPhysicalEmoteActionCol,
           //     actionMade);

            KuriTextManager.instance.Clear(KuriTextManager.PRIORITY.low);
            KuriTextManager.instance.SayRandomPositiveAffect(toa);
        }

        public void SayExerciseGoal() {
            KuriTextManager.instance.Addline(string.Join("",
                "Goal: ",
                ExerciseManager.instance.GetCurExercise().GetGoalString()),
                KuriTextManager.PRIORITY.high);
        }

        public void DoScaffoldingDialogue() {
            kuriController.TurnTowardsUser();
            ExerciseManager.instance.GetCurExercise().GetComponent<ExerciseScaffolding>().SayNextScaffold();
        }
    }
}
