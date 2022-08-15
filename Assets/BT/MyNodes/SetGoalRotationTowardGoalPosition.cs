using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class SetGoalRotationTowardGoalPosition : ActionNode {
        KuriTransformManager kuriTransformManager;
        KuriTransformManager KuriT {
            get {
                if (kuriTransformManager == null) {
                    kuriTransformManager = KuriManager.instance.GetComponent<KuriTransformManager>();
                }
                return kuriTransformManager;
            }
        }
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            Vector3 goalPoisiton = blackboard.goalPosition;
            Vector3 kuriPosition = KuriT.Position;
            Vector3 directionInKuriCords = goalPoisiton - kuriPosition;

            // get kuri forward vector in global coords
            Vector3 kuriForward = KuriT.Forward;
            Vector2 kuriFlatForward = new Vector2(kuriForward.x, kuriForward.z);
            kuriFlatForward.Normalize();
            // get angle of kuriFlatFoward relative to global coords

            float angle = Mathf.Atan2(kuriFlatForward.y, kuriFlatForward.x) * Mathf.Rad2Deg;

            // rotate direction in Kuri Cords by angle
            Vector3 direction = Quaternion.Euler(0, angle, 0) * directionInKuriCords;

            angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            blackboard.goalRotation = new Vector3(0, angle, 0);
            return State.Success;
        }
    }
}
