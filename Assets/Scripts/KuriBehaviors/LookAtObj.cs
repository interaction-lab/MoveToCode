using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class LookAtObj : ActionNode {
        Transform objToLookAt;
        KuriHeadPositionManager _kuriHeadPositionManager;
        KuriHeadPositionManager kuriHeadPositionManager {
            get {
                if (_kuriHeadPositionManager == null) {
                    _kuriHeadPositionManager = KuriHeadPositionManager.instance;
                }
                return _kuriHeadPositionManager;
            }
        }
        float headSpeed;

        protected override void OnStart() {
            objToLookAt = blackboard.objToLookAt;
            headSpeed = blackboard.headSpeed;
            if (objToLookAt == null) {
                Debug.LogError("objToLookAt is null in LookAtObj");
            }
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            Vector3 dir = objToLookAt.position - kuriHeadPositionManager.HeadPosition;
            Debug.Log(dir);
            if (dir == Vector3.zero) {
                return State.Success; // already looking at object
            }
            Quaternion rot = Quaternion.LookRotation(dir);
            kuriHeadPositionManager.HeadRotation = Quaternion.Slerp(kuriHeadPositionManager.HeadRotation,
                rot,
                headSpeed * Time.deltaTime);

            // if rotation is close enough, stop
            if (Quaternion.Angle(kuriHeadPositionManager.HeadRotation, rot) < 0.1f) {
                kuriHeadPositionManager.HeadRotation = rot;
                return State.Success;
            }
            return State.Running;
        }
    }
}
