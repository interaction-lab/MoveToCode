using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class TeleportToPose : ActionNode {
        private Vector3 goalPosition, goalRotation;
        protected override void OnStart() {
            goalPosition = blackboard.goalPosition;
            goalRotation = blackboard.goalRotation;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            context.kuriTransformManager.Position = goalPosition;
            context.kuriTransformManager.Rotation = Quaternion.Euler(
                context.kuriTransformManager.Rotation.eulerAngles.x,
                goalRotation.y,
                context.kuriTransformManager.Rotation.eulerAngles.z);
            return State.Success;
        }
    }
}
