using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class LoggableDoAnimation : LoggableBehPrimitive {
        #region members
        string animationName;
        Animator activeAnimator;
        public static LoggableDoAnimation CurDoAnim = null;
        TutorKuriManager tkManager;
        TutorKuriManager TutorKuriManagerInstance {
            get {
                if (tkManager == null) {
                    tkManager = TutorKuriManager.instance;
                }
                return tkManager;
            }
        }
        #endregion
        #region overrides
        protected override void BehCleanUp() {
            if (CurDoAnim == this) {
                CurDoAnim = null;
            }
        }

        protected override void BehSetUp() {
            CurDoAnim = this;
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
            if (CurDoAnim != this || !TutorKuriManagerInstance.IsOn) {
                return State.Success; // quietly finish
            }
            if (activeAnimator.IsThisAnimationPlaying(animationName)) {
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
