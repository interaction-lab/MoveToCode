using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode{
    public class TurnOnBodyAnimator : TurnOnAnimator {
        protected override void TurnOnAnim() {
            if(++blackboard.BodyAnimatorSemaphoreCount == 1) {
                BodyAnimator.enabled = true;
            }
        }
    }
}
