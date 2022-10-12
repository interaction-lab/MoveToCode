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
        public float TimeLastActionStarted { get; set; } = 0;
        public float TimeLastActionEnded { get; set; } = 0;
        public float TimeWindow {
            get {
                return HumanStateManager.instance.timeWindow;
            }
        }

        KuriController kuriControllerBackingVar = null;
        public KuriController KController {
            get {
                if (kuriControllerBackingVar == null) {
                    kuriControllerBackingVar = GetComponentInChildren<KuriBTBodyController>();
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
                    else if (kuriAIToUse == KuriAI.KURIAI.BehaviorTreeRand) {
                        kuriAIBackingVar = FindObjectOfType<KuriAIBTRandom>().GetComponent<KuriAIBTRandom>();
                    }
                }
                return kuriAIBackingVar;
            }
        }
        public KuriAI.KURIAI kuriAIToUse;

        bool inStartUp;
        bool wasKuriDoingActionLastTick;

        LoggingManager loggingManager;
        ViewPortManager _viewPortManager;
        ViewPortManager viewPortManager {
            get {
                if (_viewPortManager == null) {
                    _viewPortManager = ViewPortManager.instance;
                }
                return _viewPortManager;
            }
        }

        TutorKuriTransformManager tutorKuriTransformManager;
        TutorKuriTransformManager transformManager {
            get {
                if (tutorKuriTransformManager == null) {
                    tutorKuriTransformManager = TutorKuriTransformManager.instance;
                }
                return tutorKuriTransformManager;
            }
        }

        public VirtualKuriAudio KuriAudio {
            get {
                return GetComponentInChildren<VirtualKuriAudio>();
            }
        }
        #endregion

        #region unity
        private void Awake() {
            OptionSelectionManager.instance.Init();
            loggingManager = LoggingManager.instance;
            wasKuriDoingActionLastTick = KController.IsDoingAction;
            loggingManager.AddLogColumn(robotKCLevel, "");

            StartCoroutine(StartRoutine());
        }
        #endregion

        #region public
        public void AskForHelp() {
            kuriAI.ForceHelpfulAction();
        }
        public void SetKC(float kcRIn) {
            robotKC = kcRIn;
            LoggingManager.instance.UpdateLogColumn(robotKCLevel, robotKC.ToString("F3"));
        }
        public void TurnOnArrowPoint() {
            viewPortManager.TurnOnArrow(transformManager.OriginT);
        }
        public void TurnOffArrowPoint() {
            viewPortManager.TurnOffArrow(transformManager.OriginT);
        }
        #endregion

        #region private
        private void SpawnArrowPointer() {
            viewPortManager.SpawnNewArrowPoint(transformManager.OriginT, // body
                new Vector3(0, 0.1f, 0),
                Color.black, // outer color
                Color.white,  // inner color
                "Kuri Is Behind You");
            TurnOffArrowPoint();
        }
        IEnumerator StartRoutine() {
            inStartUp = true;
            yield return null;
            SpawnArrowPointer();
            KController.TurnTowardsUser();
            yield return new WaitForSeconds(5);
            KController.MoveToObj(PlayerTransformManager.instance.OriginT);
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

        void LateUpdate() {
            UpdateEndOfTickVariables();
        }

        private void Tick() {
            if (inStartUp) {
                return;
            }
            kuriAI?.Tick();
        }

        void UpdateEndOfTickVariables() {
            if (!wasKuriDoingActionLastTick && KController.IsDoingAction) {
                TimeLastActionStarted = Time.time;
            }
            else if (wasKuriDoingActionLastTick && !KController.IsDoingAction) {
                TimeLastActionEnded = Time.time;
            }
            wasKuriDoingActionLastTick = KController.IsDoingAction;
        }

        internal void MoveAway(Transform moveAwayTransform) {
            KController.MoveToObj(moveAwayTransform);
        }
        #endregion
    }
}
