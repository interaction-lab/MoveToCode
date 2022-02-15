using UnityEngine;

namespace RosSharp.RosBridgeClient {
    public class MoveTwistPublisher : Publisher<Messages.Geometry.Twist> {
        private Messages.Geometry.Twist message;

        protected override void Start() {
            base.Start();
            InitializeMessage();
        }

        private void InitializeMessage() {
            message = new Messages.Geometry.Twist();
            message.linear = new Messages.Geometry.Vector3();
            message.angular = new Messages.Geometry.Vector3();
        }
        public void UpdateMessage(float vertical, float rotationDeg) {
            message.linear.x = vertical;
            message.angular.y = rotationDeg;
            Publish(message);
        }

    }
}
