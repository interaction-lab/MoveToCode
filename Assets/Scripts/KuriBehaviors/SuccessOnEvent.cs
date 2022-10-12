using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TheKiwiCoder;

namespace MoveToCode {
    public abstract class SuccessOnEvent : MonitorCondition {
        KuriBTEventRouter eventRouter;
        public string eventName = "";
        UnityEvent evt;
        bool processedEvent = true;
        float eventLockOutTime = 0.2f; // don't allow event to be processed again until this time has passed
        float timeSinceLastEvent = 0.21f;
        protected override void OnStart() {
            eventRouter = context.eventRouter;
            if (eventName == "") {
                SetEventName();
            }

            if (evt == null) {
                evt = eventRouter.GetEvent(eventName);
                if (evt != null) { // keep trying until event exists
                    evt.AddListener(OnEvent);
                }
            }
        }

        protected abstract void SetEventName();
        protected override void OnStop() {
        }

        protected virtual void OnEvent() {
            if (timeSinceLastEvent > eventLockOutTime) {
                processedEvent = false;
                timeSinceLastEvent = 0;
            }
        }

        protected override State OnUpdate() {
            bool ret = !processedEvent;
            processedEvent = true;
            timeSinceLastEvent += Time.deltaTime;
            return ret ? State.Success : State.Failure;
        }
    }
}