using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TheKiwiCoder;

namespace MoveToCode {
    public class SuccessOnStartButtonPressed : SuccessOnEvent {
        protected override void OnStart() {
            eventName = EventNames.StartButtonPressed;
            base.OnStart();
        }
    }
}
