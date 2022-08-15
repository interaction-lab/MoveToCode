using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class RandomRotation : ActionNode {
        public float min = -180;
        public float max = 180;
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            blackboard.goalRotation.y = Random.Range(min, max);
            return State.Success;
        }
    }
}
