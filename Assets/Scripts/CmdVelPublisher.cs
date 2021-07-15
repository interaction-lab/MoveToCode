using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient {
    public class CmdVelPublisher : UnityPublisher<MessageTypes.Geometry.Twist> {

        private MessageTypes.Geometry.Twist message;
        private float previousRealTime;
        private Vector3 previousPosition = Vector3.zero;
        private Quaternion previousRotation = Quaternion.identity;

        protected override void Start() {
            base.Start();
            InitializeMessage();
        }

        private void InitializeMessage() {
            message = new MessageTypes.Geometry.Twist();
            message.linear = new MessageTypes.Geometry.Vector3();
            message.angular = new MessageTypes.Geometry.Vector3();
        }

        public void PublishCmdVel(float x, float theta) {
            message.linear.x = x;
            message.angular.y = theta;
            Publish(message);
        }
    }
}
