using RosSharp.RosBridgeClient;
using UnityEngine;

namespace MoveToCode {
    public class KuriManager : Singleton<KuriManager> {
        [Range(0.0f, 1.0f)]
        public float robotKC;

        static string rISACol = "robotISA";

        KuriEmoteStringPublisher kuriEmoteStringPublisher;
        bool doingAction, moveAroundNext;
        float timeSinceLastAction, timeWindow;

        private void Awake() {
            timeWindow = HumanStateManager.instance.timeWindow;
            timeSinceLastAction = 0;
            doingAction = false;
            moveAroundNext = true;
            kuriEmoteStringPublisher = FindObjectOfType<KuriEmoteStringPublisher>();
            LoggingManager.instance.AddLogColumn(rISACol, "");
        }

        private void Update() {
            Tick();
            timeSinceLastAction += Time.deltaTime;
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
                if (moveAroundNext) {
                    TakeMovementAction();
                }
                else {
                    TakeISAAction();
                }
                moveAroundNext = !moveAroundNext;
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
    }
}
