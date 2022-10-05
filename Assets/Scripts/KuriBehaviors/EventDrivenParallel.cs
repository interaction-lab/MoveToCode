using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MoveToCode;

namespace TheKiwiCoder {
    public class EventDrivenParallel : CompositeNode {
        protected override void OnStart() {
            // check that all children are valid EventDrivenSequence Nodes
            if (children.Any(child => !(child is EventDrivenSequence))) {
                Debug.LogError("EventDrivenParallel can only have EventDrivenSequence children");
                return;
            }
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            for (int i = 0; i < children.Count; i++) {
                children[i].Update();
            }
            return State.Running;
        }
    }
}