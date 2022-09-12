using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class SuccessOnKuriEnterViewPort : SuccessOnEvent {
        protected override void OnStart() {
            eventName = EventNames.KuriEnterViewPort;
            base.OnStart();
        }
    }
}
