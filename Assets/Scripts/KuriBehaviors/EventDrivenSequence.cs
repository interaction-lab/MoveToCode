using TheKiwiCoder;
using UnityEngine.Assertions;

namespace MoveToCode {
    public class EventDrivenSequence : Sequencer {
        SuccessOnEvent successOnEventNode;
        bool sequenceIsRunning;
        protected override void OnStart() {
            context.eventRouter.GetEvent(EventNames.OnEndAllSeq)?.AddListener(OnEndAllSeq);
            Init();
        }

        void Init() {
            current = 1;
            sequenceIsRunning = false;
            successOnEventNode = children[0] as SuccessOnEvent;
            Assert.IsTrue(successOnEventNode != null, "EventDrivenSequence must have a SuccessOnEvent as its first child");
        }

        void OnEndAllSeq() {
            EndSequence();
        }

        protected override void OnStop() {
            context.eventRouter.GetEvent(EventNames.OnEndAllSeq)?.RemoveListener(OnEndAllSeq);
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
                EndSequence();
                Init();
                sequenceIsRunning = true;
            }
        }

        private void EndSequence() {
            if (sequenceIsRunning) {
                children[current].Abort();
            }
            sequenceIsRunning = false;
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
    }
}
