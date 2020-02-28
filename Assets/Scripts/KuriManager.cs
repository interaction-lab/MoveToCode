using RosSharp.RosBridgeClient;
using UnityEngine;

namespace MoveToCode {
    public class KuriManager : Singleton<KuriManager> {
        [Range(0.0f, 1.0f)]
        public float robotKC;

        static string rISACol = "robotISA";

        KuriEmoteStringPublisher kuriEmoteStringPublisher;
        float timeSinceLastAction, timeWindow;

        private void Awake() {
            timeWindow = HumanStateManager.instance.timeWindow;
            timeSinceLastAction = 0;
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
    }
}
