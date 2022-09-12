using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode{
    public class AtDestinationCondition : MonitorCondition
    {
        TutorKuriTransformManager _kuriTransformManager;
        TutorKuriTransformManager kuriTransformManager {
            get {
                if (_kuriTransformManager == null) {
                    _kuriTransformManager = TutorKuriTransformManager.instance;
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
