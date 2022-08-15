using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class IsNextPositionUser : MonitorCondition {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            return context.agent.steeringTarget == context.agent.destination ? State.Success : State.Failure;
        }
    }
}
