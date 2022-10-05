using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TheKiwiCoder;

namespace MoveToCode {
    public abstract class SuccessOnEvent : MonitorCondition {
        KuriBTEventRouter _eventRouter;
        KuriBTEventRouter eventRouter {
            get {
                if (_eventRouter == null) {
                    _eventRouter = KuriBTEventRouter.instance;
                }
                return _eventRouter;
            }
        }
        public string eventName = "";
        UnityEvent evt;

        float timeEventHappened = -1f, lastFrameTime = -1f;
        bool processedEvent = true;

        protected override void OnStart() {
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
            timeEventHappened = Time.time;
            processedEvent = false;
        }

        protected override State OnUpdate() {
            bool ret = false;
            if (timeEventHappened == Time.time || // happened this frame
               (timeEventHappened == lastFrameTime && !processedEvent)) { // happened last frame and was not processed
                processedEvent = true;
                ret = true;
            }
            lastFrameTime = Time.time;
            return ret ? State.Success : State.Failure;
        }
    }
}
