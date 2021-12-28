using RosSharp.RosBridgeClient;
using System.Collections;
using UnityEngine;
using static MoveToCode.KuriController;

namespace MoveToCode {
    public class BabyKuriManager : Singleton<BabyKuriManager> {
        [Range(-3.0f, 3.0f)]
        public float robotKC;
        [HideInInspector]
        static string robotKCLevel = "robotKCLevel";

        PoseStampedPublisher poseStampPublisher;

        public float TimeLastActionStarted { get; set; } = 0;
        public float TimeLastActionEnded { get; set; } = 0;
        public float TimeWindow {
            get {
                return HumanStateManager.instance.timeWindow;
            }
        }

        public Transform KuriGoalPoseTransform;

        KuriController kuriControllerBackingVar = null;
        public KuriController kuriController {
            get {
                if (kuriControllerBackingVar == null) {
                    FindObjectOfType<PhysicalKuriController>().GetComponent<PhysicalKuriController>().enabled = false;
                    kuriControllerBackingVar = FindObjectOfType<VirtualKuriController>().GetComponent<VirtualKuriController>();
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

        LoggingManager loggingManager;

        private void Awake() {
            OptionSelectionManager.instance.Init();
            loggingManager = LoggingManager.instance;
            wasKuriDoingActionLastTick = kuriController.IsDoingAction;
            loggingManager.AddLogColumn(robotKCLevel, "");
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
            yield return new WaitForSeconds(0.1f);
            kuriController.DoAnimationAction(EMOTIONS.close_eyes);
            yield return new WaitForSeconds(InteractionManager.instance.MinToSeconds(InteractionManager.instance.warmUpTimeMinutes) - 3f);
            kuriController.DoAnimationAction(EMOTIONS.happy);
            inStartUp = false;
        }

        private void Update() {
            Tick();
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
    }
}
