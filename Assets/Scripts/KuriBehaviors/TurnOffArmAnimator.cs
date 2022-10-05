using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class TurnOffArmAnimator : TurnOffAnimator {
        protected override void TurnOffAnim() {
            ArmAnimator.enabled = false;
        }
    }
}
