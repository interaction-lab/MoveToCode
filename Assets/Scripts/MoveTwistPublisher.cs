using UnityEngine;

namespace RosSharp.RosBridgeClient {
    public class MoveTwistPublisher : UnityPublisher<MessageTypes.Geometry.Twist> {
        private MessageTypes.Geometry.Twist message;

        protected override void Start() {
            base.Start();
            InitializeMessage();
        }

        private void InitializeMessage() {
            message = new MessageTypes.Geometry.Twist();
            message.linear = new MessageTypes.Geometry.Vector3();
            message.angular = new MessageTypes.Geometry.Vector3();
        }
        public void UpdateMessage(float vertical, float rotationDeg) {
            message.linear.x = vertical;
            message.angular.y = message.angular.x = message.angular.z = Mathf.Deg2Rad *  rotationDeg;
            Publish(message);
        }

    }
}
