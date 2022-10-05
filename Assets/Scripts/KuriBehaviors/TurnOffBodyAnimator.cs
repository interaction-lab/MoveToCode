using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode{
    public class TurnOffBodyAnimator : TurnOffAnimator {
        protected override void TurnOffAnim() {
            BodyAnimator.enabled = false;
        }
    }
}
