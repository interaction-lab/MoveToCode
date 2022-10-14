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
        List<MeshRenderer> _meshrends = null;
        List<MeshRenderer> KuriMeshRends {
            get {
                if (_meshrends == null) {
                    _meshrends = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>(true));
                }
                return _meshrends;
            }
        }
        public bool IsOn = true;
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
            if (myArrowPoint != null) {
                viewPortManager.TurnOnArrow(transformManager.OriginT);
            }
        }
        public void TurnOffArrowPoint() {
            if (myArrowPoint != null) {
                viewPortManager.TurnOffArrow(transformManager.OriginT);
            }
        }
        public bool IsKuriOnScreen() {
            if (myArrowPoint == null) {
                return true; // shouldn't happen but whatever
            }
            return myArrowPoint.IsInViewPort;
        }
        #endregion

        #region private
        ArrowPointPrefab myArrowPoint;
        private void SpawnArrowPointer() {
            myArrowPoint = viewPortManager.SpawnNewArrowPoint(transformManager.OriginT, // body
                new Vector3(0, 0.1f, 0),
                Color.black, // outer color
                Color.white,  // inner color
                "Kuri Is Behind You");
            TurnOffArrowPoint();
            myArrowPoint.OnEnterViewPort.AddListener(TurnOffArrowPoint);
        }
        IEnumerator StartRoutine() {
            inStartUp = true;
            yield return null;
            SpawnArrowPointer();
            KController.TurnTowardsUser();
            yield return new WaitForSeconds(5);
            KController.MoveToObj(PlayerTransformManager.instance.OriginT);
            SetKuriVisibility(false);
            yield return new WaitForSeconds(InteractionManager.instance.MinToSeconds(InteractionManager.instance.warmUpTimeMinutes) - 5f);
            inStartUp = false;
        }

        public void SetKuriVisibility(bool b) {
            SetMREnabledRecurssive(transform, b);
            KuriAudio.enabled = b;
            IsOn = b;
        }

        void SetMRsOfAllChildrenTransforms(Transform[] goArr, bool on) {
            foreach (Transform go in goArr) {
                SetMREnabledRecurssive(go, on);
            }
        }
        HashSet<string> MRsNotToTurnOffSet = new HashSet<string>() { "MazePaper", "KuriTextManager" };
        void SetMREnabledRecurssive(Transform go, bool on) {
            if (MRsNotToTurnOffSet.Contains(go.name)) {
                return; // don't turn these off or any of their children
            }

            if (go.transform.childCount > 0) {
                List<Transform> goArr = new List<Transform>();
                foreach (Transform t in go.transform) {
                    goArr.Add(t);
                }
                SetMRsOfAllChildrenTransforms(goArr.ToArray(), on);
            }

            MeshRenderer rend = go.GetComponent<MeshRenderer>();
            if (rend) {
                rend.enabled = on;
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
