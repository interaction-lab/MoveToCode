using MoveToCode;
using UnityEngine;
using static MoveToCode.KuriController;

namespace RosSharp.RosBridgeClient {
    public class KuriEmoteStringPublisher : Publisher<Messages.Standard.String> {

        private Messages.Standard.String message;

        protected override void Start() {
            base.Start();
            InitializeMessage();

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
            Publish(message);
        }

        private void InitializeMessage() {
            message = new Messages.Standard.String {
                data = EMOTIONS.happy.ToString()
            };
        }

    }
}
