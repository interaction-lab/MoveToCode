using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode{
    public class FloatUpward : ActionNode
    {
        public float timeToFloat;
        public float distance;

        private float startTime;
        private float speed;
        private TutorKuriTransformManager tkTransformManager;
        
        protected override void OnStart() {
            startTime = Time.time;
            tkTransformManager = TutorKuriTransformManager.instance;
            speed = distance / timeToFloat;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (Time.time - startTime > timeToFloat) {
                return State.Success;
            }
            // move kuri upward over time
            tkTransformManager.Position = tkTransformManager.Position + Vector3.up * speed;
            return State.Running;
        }
    }
}
