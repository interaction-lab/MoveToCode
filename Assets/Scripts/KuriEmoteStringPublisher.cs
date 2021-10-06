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

        public string PubRandomPositive() {
            int choice = Random.Range(0, PositiveEmotions.Length);
            PublishAction(PositiveEmotions[choice]);
            return PositiveEmotions[choice].ToString();
        }
        public string PubRandomNegative() {
            int choice = Random.Range(0, NegativeEmotions.Length);
            PublishAction(NegativeEmotions[choice]);
            return NegativeEmotions[choice].ToString();
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
