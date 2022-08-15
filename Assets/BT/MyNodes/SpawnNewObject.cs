using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class SpawnNewObject : ActionNode {
        ObjectToPickUpManager _objectToPickUpManager;
        ObjectToPickUpManager objectToPickUpManager {
            get {
                if (_objectToPickUpManager == null) {
                    _objectToPickUpManager = ObjectToPickUpManager.instance;
                }
                return _objectToPickUpManager;
            }
        }
        protected override void OnStart() {
            objectToPickUpManager.SpawnCurObject(blackboard.goalPosition);
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            return State.Success;
        }
    }
}
