using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE{
    public class AtDestinationCondition : MonitorCondition
    {
        KuriTransformManager _kuriTransformManager;
        KuriTransformManager kuriTransformManager {
            get {
                if (_kuriTransformManager == null) {
                    _kuriTransformManager = KuriManager.instance.GetComponent<KuriTransformManager>();
                }
                return _kuriTransformManager;
            }
        }
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if(blackboard.goalPosition == kuriTransformManager.Position)
            {
                return State.Success;
            }
            else
            {
                return State.Failure;
            }
        }
    }
}
