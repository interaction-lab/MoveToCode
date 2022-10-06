using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class LookAtObj : ActionNode {
        Transform objToLookAt, origTransform;
        KuriHeadPositionManager _kuriHeadPositionManager;
        TutorKuriTransformManager kuriTransformManager;
        KuriBTBodyController kuriBodyController;
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
            Init();
        }

        void Init() {
            objToLookAt = blackboard.objToLookAt;
            origTransform = objToLookAt;
            headSpeed = blackboard.headSpeed;
            kuriTransformManager = context.kuriTransformManager;
            kuriBodyController = context.KController as KuriBTBodyController;
            if (objToLookAt == null) {
                Debug.LogError("objToLookAt is null in LookAtObj");
            }
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (objToLookAt != origTransform) {
                Init();
            }

            if (!kuriTransformManager.IsWithinHeadPanConstraints()) {
                // tell kuri controller to turn at this object
                kuriBodyController.OnlyTurnToObj(objToLookAt);
            }

            Vector3 dir = objToLookAt.position - kuriHeadPositionManager.HeadPosition;
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
