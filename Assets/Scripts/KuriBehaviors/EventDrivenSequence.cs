using TheKiwiCoder;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace MoveToCode {
    public class EventDrivenSequence : Sequencer {
        #region members
        SuccessOnEvent successOnEventNode;
        UnityEvent endSeqEvt = null;
        bool seqRestartingOnNewEventCall = false, eventTriggeredAtLeastOnce = false;
        #endregion
        #region bt
        protected override void OnStart() {
            Init();
        }

        protected override void OnStop() {
            eventTriggeredAtLeastOnce = false;
        }

        protected override State OnUpdate() {
            RestartSequenceOnEvent();
            if (eventTriggeredAtLeastOnce) {
                return RunChildren();
            }
            return State.Running;
        }
        #endregion

        #region helpers

        void Init() {
            Assert.IsFalse(seqRestartingOnNewEventCall, "EventDrivenSequence must be stopped before starting again");
            if (endSeqEvt == null) {
                endSeqEvt = context.eventRouter.GetEvent(EventNames.OnEndAllSeq);
                endSeqEvt.AddListener(EndSeq);
            }
            current = 1;
            successOnEventNode = children[0] as SuccessOnEvent;
            Assert.IsTrue(successOnEventNode != null, "EventDrivenSequence must have a SuccessOnEvent as its first child");
        }

        private void RestartSequenceOnEvent() {
            if (successOnEventNode.Update() == State.Success) {
                eventTriggeredAtLeastOnce = true;
                seqRestartingOnNewEventCall = true;
                ResetSeq();
            }
            else {
                seqRestartingOnNewEventCall = false;
            }
        }

        void EndSeq() {
            Abort();
        }

        private void ResetSeq() {
            Assert.IsTrue(seqRestartingOnNewEventCall);
            children[current].Abort();
            seqRestartingOnNewEventCall = false;
            Init();
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
        #endregion
    }
}
