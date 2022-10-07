using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.Assertions;

namespace MoveToCode {
    public class EventDrivenSequence : Sequencer {
        SuccessOnEvent successOnEventNode;
        bool sequenceIsRunning;
        protected override void OnStart() {
            Init();
        }

        void Init() {
            current = 1;
            sequenceIsRunning = false;
            successOnEventNode = children[0] as SuccessOnEvent;
            Assert.IsTrue(successOnEventNode != null, "EventDrivenSequence must have a SuccessOnEvent as its first child");
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            RestartSequenceOnEvent();

            if (sequenceIsRunning) {
                State ret = RunChildren();
                if (ret != State.Success) {
                    return ret;
                }
            }
            sequenceIsRunning = false;
            return State.Success;
        }

        private void RestartSequenceOnEvent() {
            if (successOnEventNode.Update() == State.Success) {
                // check if sequence is already running,
                // abort if so and start over
                if (sequenceIsRunning) {
                    children[current].Abort();
                    RunRemainingLogAndAnimatorNodes();
                    Init();
                }
                sequenceIsRunning = true;
            }
        }

        private State RunChildren() {
            for (int i = current; i < children.Count; ++i) {
                current = i;
                var child = children[current];

                switch (child.Update()) {
                    case State.Running:
                        return State.Running;
                    case State.Failure:
                        return State.Failure;
                    case State.Success:
                        continue;
                }
            }
            return State.Success;
        }

        private void RunRemainingLogAndAnimatorNodes() {
            for (int i = current + 1; i < children.Count; ++i) {
                if (children[i] is LogActionEnded || children[i] is TurnOnAnimator || children[i] is TurnOffAnimator) {
                    children[i].Update();
                }
            }
        }
    }
}
