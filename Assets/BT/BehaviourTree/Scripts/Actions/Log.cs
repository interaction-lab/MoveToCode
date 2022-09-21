using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    public class Log : ActionNode
    {
        public string message;

        protected override void OnStart() {
            Debug.Log("Start");
        }

        protected override void OnStop() {
            Debug.Log("End");
        }

        protected override State OnUpdate() {
            Debug.Log($"{message}");
            return State.Success;
        }
    }
}
