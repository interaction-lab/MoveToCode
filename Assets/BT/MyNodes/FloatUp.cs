using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using MoveToCode;

namespace NRISVTE{
    public class FloatUp : ActionNode
    {
        public float distance = 1;
        public float duration = 1;
        float startTime;
        TutorKuriTransformManager tk;

        protected override void OnStart() {
            startTime = Time.time;
            tk = TutorKuriTransformManager.instance;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if(Time.time - startTime > duration) {
                return State.Success;
            }
            tk.Position = tk.Position + Vector3.up * (distance/duration * Time.deltaTime);
            return State.Success;
        }
    }
}
