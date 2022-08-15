using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class MonitorPublicBool : MonitorCondition {
        public bool value;

        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            return value ? State.Success : State.Failure;
        }
    }
}
