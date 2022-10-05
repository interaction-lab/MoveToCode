using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode{
    public class TurnOnArmAnimator : TurnOnAnimator {
        protected override void TurnOnAnim() {
            ArmAnimator.enabled = true;
        }
    }
}
