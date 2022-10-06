using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode{
    public class TurnOnArmAnimator : TurnOnAnimator {
        protected override void TurnOnAnim() {
            if(++blackboard.ArmAnimatorSemaphoreCount == 1) {
                ArmAnimator.enabled = true;
            }
        }
    }
}
