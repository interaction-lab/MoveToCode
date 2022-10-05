using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode{
    public class SuccessOnLookAtObj : SuccessOnEvent {
        protected override void SetEventName() {
            eventName = EventNames.OnLookAtObj;
        }
    }
}
