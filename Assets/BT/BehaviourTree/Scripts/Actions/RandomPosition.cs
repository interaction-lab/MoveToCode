using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class RandomPosition : ActionNode {
        public Vector2 min = Vector2.one * -10;
        public Vector2 max = Vector2.one * 10;
        TutorKuriTransformManager _kuriTransformManager;
        TutorKuriTransformManager KuriT {
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
            blackboard.goalPosition.x = Random.Range(min.x, max.x);
            blackboard.goalPosition.y = KuriT.GroundYCord;
            blackboard.goalPosition.z = Random.Range(min.y, max.y);
            return State.Success;
        }
    }
}