using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class SetPoseToPlayer : ActionNode {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            PlayerTransformManager playerTransformManager = Camera.main.GetComponent<PlayerTransformManager>();
            KuriTransformManager kuriTransformManager = KuriManager.instance.GetComponent<KuriTransformManager>();
            blackboard.goalPosition = playerTransformManager.Position;
            blackboard.goalPosition.y = kuriTransformManager.GroundYCord;
            blackboard.goalRotation = Quaternion.LookRotation(playerTransformManager.Position - kuriTransformManager.Position, Vector3.up).eulerAngles;
            return State.Success;
        }
    }
}
