using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class DoAnimation : ActionNode {
        string animationName;
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
        Animator activeAnimator;
        protected override void OnStart() {
            animationName = blackboard.emotion.ToString();
            // check if animation is in body animator
            if(IsAnimationInAnimator(BodyAnimator, animationName)) {
                activeAnimator = BodyAnimator;
                BodyAnimator.Play(animationName);
            }
            else{
                activeAnimator = ArmAnimator;
                ArmAnimator.Play(animationName);
            }
        }

        bool IsAnimationInAnimator(Animator animator, string animationName) {
            if (animator.runtimeAnimatorController.animationClips.Length > 0) {
                foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) {
                    if (clip.name == animationName) {
                        return true;
                    }
                }
            }
            return false;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (activeAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == animationName) {
                return State.Running;
            }
            return State.Success;
        }
    }
}
