using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class TurnToObj : ActionNode {
        Transform objTransform;
        TutorKuriTransformManager kuriTransformManager;
        float turnSpeed = 1f;
        protected override void OnStart() {
            objTransform = blackboard.objToTurnTo;
            kuriTransformManager = context.kuriTransformManager;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (objTransform == null) {
                return State.Failure;
            }
            Vector3 objPos = objTransform.position;
            Vector3 kuriPos = kuriTransformManager.Position;
            objPos.y = kuriPos.y;
            Vector3 targetDir = objPos - kuriPos;
            if (Vector3.Angle(kuriTransformManager.Forward, targetDir) < 1f) {
                return State.Success;
            }

            float step = turnSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(kuriTransformManager.Forward, targetDir, step, 0.0f);
            kuriTransformManager.Rotation = Quaternion.LookRotation(newDir);


            return State.Running;
        }
    }
}
