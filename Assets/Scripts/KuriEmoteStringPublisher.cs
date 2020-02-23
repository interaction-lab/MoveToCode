using MoveToCode;
using UnityEngine;

namespace RosSharp.RosBridgeClient {
    public class KuriEmoteStringPublisher : Publisher<Messages.Standard.String> {

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

        private Messages.Standard.String message;
        string kuriPhysicalEmoteActionCol = "kuriPhysicalAction";
        protected override void Start() {
            base.Start();
            InitializeMessage();
            LoggingManager.instance.AddLogColumn(kuriPhysicalEmoteActionCol, "");
        }

        void LogAction(EMOTIONS e) {
            LoggingManager.instance.UpdateLogColumn(kuriPhysicalEmoteActionCol, e.ToString());
        }

        public void PubRandomPositive() {
            EMOTIONS[] e = new EMOTIONS[] { EMOTIONS.happy, EMOTIONS.love, EMOTIONS.thinking };
            int choice = Random.Range(0, 3);
            PublishAction(e[choice]);
        }
        public void PubRandomNegative() {
            EMOTIONS[] e = new EMOTIONS[] { EMOTIONS.sad, EMOTIONS.sassy, EMOTIONS.confused };
            int choice = Random.Range(0, 3);
            PublishAction(e[choice]);
        }

        public void PubNeutralAction() {
            PublishAction(EMOTIONS.neutral);
        }

        public void PublishAction(EMOTIONS action) {
            message.data = action.ToString();
            LogAction(action);
            Publish(message);
        }

        private void InitializeMessage() {
            message = new Messages.Standard.String {
                data = "happy"
            };
        }

    }
}
