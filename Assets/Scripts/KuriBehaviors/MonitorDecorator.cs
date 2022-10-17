using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    public class MonitorDecorator : DecoratorNode {
        public override Node Clone() {
            return base.Clone();
        }

        public override bool Equals(object other) {
            return base.Equals(other);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override void OnDrawGizmos() {
            base.OnDrawGizmos();
        }

        public override string ToString() {
            return base.ToString();
        }

        protected override void OnStart() {

        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            return child.Update();
        }
    }
}