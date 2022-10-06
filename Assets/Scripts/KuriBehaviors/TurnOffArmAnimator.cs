using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class TurnOffArmAnimator : TurnOffAnimator {
        protected override void TurnOffAnim() {
            if(--blackboard.ArmAnimatorSemaphoreCount == 0) {
                ArmAnimator.enabled = false;
            }
        }
    }
}
