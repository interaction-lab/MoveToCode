using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class TurnToPose : ActionNode {
        float turnSpeed = 120f; // degrees per second
        float goalYRot;
        float minAngle;
        TutorKuriTransformManager kuriTransformManager;
        TutorKuriTransformManager KuriT {
            get {
                if (kuriTransformManager == null) {
                    kuriTransformManager = TutorKuriTransformManager.instance;
                }
                return kuriTransformManager;
            }
        }
        protected override void OnStart() {
            minAngle = turnSpeed * Time.deltaTime * 2f;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            Vector3 goalPosition = blackboard.goalPosition;
            Vector3 kuriPosition = KuriT.Position;
            Vector3 directionInKuriCords = goalPosition - kuriPosition;
            Vector3 kuriForward = KuriT.Forward;
            // get angle between directionInKuriCords and kuriForward
            float angle = Vector3.SignedAngle(directionInKuriCords, kuriForward, Vector3.up); // y angle to rotate kuri by
            float timedSpeed = turnSpeed * Time.deltaTime;
            Vector3 newForward = Vector3.RotateTowards(KuriT.Forward, directionInKuriCords, timedSpeed, 0.0f);
            KuriT.Rotation = Quaternion.LookRotation(newForward);

            return angle < minAngle ? State.Success : State.Running;
        }
    }
}
