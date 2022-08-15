using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TheKiwiCoder;

namespace NRISVTE {
    public class SuccessOnStopButtonPressed : SuccessOnEvent {
        protected override void OnStart() {
            eventName = EventNames.StopButtonPressed;
            base.OnStart();
        }
    }
}
