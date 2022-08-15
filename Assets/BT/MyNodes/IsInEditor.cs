using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE{
    public class IsInEditor : ActionNode
    {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            #if UNITY_EDITOR
                return State.Success;
            #else
                return State.Failure;
            #endif
            return State.Failure;
        }
    }
}
