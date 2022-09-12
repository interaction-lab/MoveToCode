using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class WaitForServerResponse : SuccessOnEvent {
        protected override void OnStart() {
            eventName = EventNames.ReceivedMessage;
            base.OnStart();
        }
    }
}
