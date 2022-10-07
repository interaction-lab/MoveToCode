using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class WaitUntilAnimatorsOn : ActionNode {
        float timeToWait = 5f, timeWaited = 0f;
        Animator bodyAnimator, armAnimator;
        Animator BodyAnimator {
            get {
                if (bodyAnimator == null) {
                    bodyAnimator = context.mainAnimator;
                }
                return bodyAnimator;
            }
        }
        Animator ArmAnimator {
            get {
                if (armAnimator == null) {
                    armAnimator = context.armAnimator;
                }
                return armAnimator;
            }
        }
        protected override void OnStart() {
            timeWaited = 0f;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (timeWaited > timeToWait) {
                return State.Failure;
            }
            if (BodyAnimator.enabled && ArmAnimator.enabled) {
                return State.Success;
            }
            timeWaited += Time.deltaTime;
            return State.Running;
        }
    }
}
