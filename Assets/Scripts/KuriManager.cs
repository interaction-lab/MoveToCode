using RosSharp.RosBridgeClient;
using UnityEngine;

namespace MoveToCode {
    public class KuriManager : Singleton<KuriManager> {
        [Range(0.0f, 1.0f)]
        public float robotKC;

        static string rISACol = "robotISA";

        KuriEmoteStringPublisher kuriEmoteStringPublisher;
        PoseStampedPublisher poseStampPublisher;
        float timeSinceLastAction, timeWindow;

        Transform kuriGoalPoseTransform, kuriCurPoseTransform;

        private void Awake() {
            timeWindow = HumanStateManager.instance.timeWindow;
            timeSinceLastAction = 0;
            kuriEmoteStringPublisher = FindObjectOfType<KuriEmoteStringPublisher>();
            poseStampPublisher = FindObjectOfType<PoseStampedPublisher>();
            LoggingManager.instance.AddLogColumn(rISACol, "");
        }

        private void Update() {
            Tick();
            timeSinceLastAction += Time.deltaTime;
        }

        public Transform GetKuriGoalPoseTransform() {
            if (kuriGoalPoseTransform == null) {
                kuriGoalPoseTransform = transform.GetChild(0);
            }
            return kuriGoalPoseTransform;
        }

        public Transform GetKuriCurPoseTransform() {
            if (kuriCurPoseTransform == null) {
                kuriCurPoseTransform = transform.GetChild(1);
            }
            return kuriCurPoseTransform;
        }

        public void AlertActionMade() {
            timeSinceLastAction = 0;
        }

        private void Tick() {
            if (timeSinceLastAction < timeWindow) {
                return;
            }
            float kctS = HumanStateManager.instance.GetKCt();
            if (kctS < robotKC) {
                TakeMovementAction();
                TakeISAAction();
            }
            else {
                kuriEmoteStringPublisher?.PubRandomPositive();
            }
            AlertActionMade();
        }

        private void TakeISAAction() {
            Debug.Log("ISA action taken");
            LoggingManager.instance.UpdateLogColumn(rISACol, "action");
        }

        private void TakeMovementAction() {
            Debug.Log("Movement action taken");
        }

        public void SayAndDoPositiveAffect() {
            Debug.Log("Positive affect");
            poseStampPublisher?.PubTurnTowardUser();
            kuriEmoteStringPublisher?.PubRandomPositive();
            AlertActionMade();
        }

        public void SayExerciseGoal(Exercise ex) {
            poseStampPublisher?.PubTurnTowardUser();
            Debug.Log("Say " + ex.ToString());
            AlertActionMade();
        }

        public void DoVirtualTaskAssistAndScafflding(Exercise ex) {
            poseStampPublisher?.PubTurnTowardUser();
            Debug.Log("Virtual task assist and scaffold " + ex.ToString());
            AlertActionMade();
        }
    }
}
