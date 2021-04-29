﻿/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using MoveToCode;
using UnityEngine;

namespace RosSharp.RosBridgeClient {
    public class PoseStampedPublisher : Publisher<Messages.Geometry.PoseStamped> {
        public string FrameId = "map";
        static string poseGoalCol = "poseGoalSent";
        private Messages.Geometry.PoseStamped message;
        bool initialized = false;
        public Vector3 offset;

        protected override void Start() {
            base.Start();
            InitializeMessage();
            //LoggingManager.instance.AddLogColumn(poseGoalCol, "");
            initialized = true;
        }

        private void InitializeMessage() {
            message = new Messages.Geometry.PoseStamped {
                header = new Messages.Standard.Header() {
                    frame_id = FrameId
                }
            };
        }

        private void PublishMessage(Vector3 lin, Quaternion ang) {
            if (!initialized) {
                return;
            }
            lin = lin + offset;
            message.header.Update();
            message.pose.position = GetGeometryPoint(lin.Unity2Ros());
            message.pose.orientation = GetGeometryQuaternion(ang.Unity2Ros());

            //Publish(message);
            //LoggingManager.instance.UpdateLogColumn(poseGoalCol,
            //    string.Join(";", lin.ToString("F3").Replace(',', ';'), ang.ToString("F3").Replace(',', ';')));
        }

        public void PublishPosition(Vector3 lin, Quaternion ang) {
            PublishMessage(lin, ang);
        }

        public void PublishPosition(Transform t) {
            PublishPosition(t.position, t.rotation);
        }

        public void PubTurnTowardUser() {
            Quaternion rotationGoal = Quaternion.LookRotation(Camera.main.transform.forward);
            Vector3 curPos = KuriManager.instance.KuriGoalPoseTransform.position;
            PublishPosition(curPos, rotationGoal);
        }

        private Messages.Geometry.Point GetGeometryPoint(Vector3 position) {
            Messages.Geometry.Point geometryPoint = new Messages.Geometry.Point();
            geometryPoint.x = position.x;
            geometryPoint.y = position.y;
            geometryPoint.z = position.z;
            return geometryPoint;
        }

        private Messages.Geometry.Quaternion GetGeometryQuaternion(Quaternion quaternion) {
            Messages.Geometry.Quaternion geometryQuaternion = new Messages.Geometry.Quaternion();
            geometryQuaternion.x = quaternion.x;
            geometryQuaternion.y = quaternion.y;
            geometryQuaternion.z = quaternion.z;
            geometryQuaternion.w = quaternion.w;
            return geometryQuaternion;
        }

    }
}
