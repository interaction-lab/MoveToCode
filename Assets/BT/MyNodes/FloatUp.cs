using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode{
    public class FloatUp : ActionNode
    {
        public float distance;
        public float timetofloat;
        
        float startTime;
        TutorKuriTransformManager tk;
        protected override void OnStart() {
            tk = TutorKuriTransformManager.instance;
            startTime = Time.time;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (Time.time - startTime < timetofloat) {
                tk.Position = tk.Position + (Vector3.up * (distance / timetofloat * Time.deltaTime));
                return State.Running;
            }
            return State.Success;
        }
    }
}
