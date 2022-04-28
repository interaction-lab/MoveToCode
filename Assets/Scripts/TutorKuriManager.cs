using Microsoft.MixedReality.Toolkit.UI;
using RosSharp.RosBridgeClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MoveToCode.KuriController;

namespace MoveToCode {
    public class TutorKuriManager : Singleton<TutorKuriManager> {
        #region members
        [Range(-3.0f, 3.0f)]
        public float robotKC;
        [HideInInspector]
        public bool usePhysicalKuri = true;
        static string robotKCLevel = "robotKCLevel";

        PoseStampedPublisher poseStampPublisher;

        public float TimeLastActionStarted { get; set; } = 0;
        public float TimeLastActionEnded { get; set; } = 0;
        public float TimeWindow {
            get {
                return HumanStateManager.instance.timeWindow;
            }
        }

        KuriController kuriControllerBackingVar = null;
        public KuriController kuriController {
            get {
                if (kuriControllerBackingVar == null) {
                    if (usePhysicalKuri) {
                        FindObjectOfType<VirtualKuriController>().GetComponent<VirtualKuriController>().enabled = false;
                        kuriControllerBackingVar = FindObjectOfType<PhysicalKuriController>().GetComponent<PhysicalKuriController>();
                    }
                    else {
                        FindObjectOfType<PhysicalKuriController>().GetComponent<PhysicalKuriController>().enabled = false;
                        kuriControllerBackingVar = FindObjectOfType<VirtualKuriController>().GetComponent<VirtualKuriController>();
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
                        kuriAIBackingVar = FindObjectOfType<KuriRuleBasedAI>().GetComponent<KuriRuleBasedAI>();
                    }
                }
                return kuriAIBackingVar;
            }
        }
        public KuriAI.KURIAI kuriAIToUse;

        bool inStartUp;
        bool wasKuriDoingActionLastTick;

        LoggingManager loggingManager;
        #endregion

        #region unity
        private void Awake() {
            OptionSelectionManager.instance.Init();
            loggingManager = LoggingManager.instance;
            wasKuriDoingActionLastTick = kuriController.IsDoingAction;
            loggingManager.AddLogColumn(robotKCLevel, "");
        }

        private void Start() {
            StartCoroutine(StartRoutine());
        }
        #endregion

        #region public
        public void SetKC(float kcRIn) {
            robotKC = kcRIn;
            LoggingManager.instance.UpdateLogColumn(robotKCLevel, robotKC.ToString("F3"));
        }
        #endregion

        #region private
        IEnumerator StartRoutine() {
            inStartUp = true;
            yield return null;
            if (!usePhysicalKuri) {
                kuriController.GetComponent<VirtualKuriController>().TurnTowardsUser();
            }
            yield return new WaitForSeconds(5);
            if (!usePhysicalKuri) {
                kuriController.GetComponent<VirtualKuriController>().MoveToUser();
            }
            yield return new WaitForSeconds(InteractionManager.instance.MinToSeconds(InteractionManager.instance.warmUpTimeMinutes) - 5f);
            inStartUp = false;
        }

        void TurnOffAllMeshRenderers(Transform[] goArr) {
            foreach (Transform go in goArr) {
                TurnOffMeshRenderers(go);
            }
        }

        void TurnOffMeshRenderers(Transform go) {
            if (go.name == "KuriArms") {
                return; // don't turn these off
            }

            if (go.transform.childCount > 0) {
                List<Transform> goArr = new List<Transform>();
                foreach (Transform t in go.transform) {
                    goArr.Add(t);
                }
                TurnOffAllMeshRenderers(goArr.ToArray());
            }

            MeshRenderer rend = go.GetComponent<MeshRenderer>();
            if (rend) {
                rend.enabled = false;
            }
        }

        private void Update() {
            Tick();
        }

        private void Tick() {
            if (inStartUp) {
                return;
            }
            kuriAI?.Tick();
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
        #endregion
    }
}
