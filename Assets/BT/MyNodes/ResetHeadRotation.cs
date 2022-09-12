using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class ResetHeadRotation : ActionNode {
        KuriHeadPositionManager _kuriHeadPositionManager;
        KuriHeadPositionManager kuriHeadPositionManager {
            get {
                if (_kuriHeadPositionManager == null) {
                    _kuriHeadPositionManager = KuriHeadPositionManager.instance;
                }
                return _kuriHeadPositionManager;
            }
        }
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            kuriHeadPositionManager.transform.localRotation = Quaternion.identity;
            return State.Success;
        }
    }
}
