using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class LoggableDoAnimation : LoggableBehPrimitive {
        #region members
        string animationName;
        Animator activeAnimator;
        #endregion
        #region overrides
        protected override void BehCleanUp() {
        }

        protected override void BehSetUp() {
            animationName = blackboard.emotion.ToString();
            // check if animation is in body animator
            if (BodyAnimator.IsAnimationInAnimator(animationName)) {
                activeAnimator = BodyAnimator;
                BodyAnimator.Play(animationName);
            }
            else {
                activeAnimator = ArmAnimator;
                ArmAnimator.Play(animationName);
            }
        }

        protected override State OnUpdate() {
            if (activeAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == animationName) {
                return State.Running;
            }
            return State.Success;
        }

        protected override void SetAnimatorSemaphoreCount() {
            AddToBodyAnimatorSemaphore = -1; // animator takes negative side of the semaphore
            AddToArmAnimatorSemaphore = -1; // animator takes negative side of the semaphore
        }

        protected override void SetLogActionName() {
            actionName = string.Join(Separator,
                EventNames.OnDoAnimation,
                blackboard.emotion.ToString());
        }
        #endregion
        #region helpers
        #endregion
    }
}
