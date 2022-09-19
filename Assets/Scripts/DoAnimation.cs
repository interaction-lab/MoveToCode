using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE{
    public class DoAnimation : ActionNode
    {
        public string animationName;
        Animator animator;
        protected override void OnStart() {
            animator = context.gameObject.GetComponent<Animator>();
            animator.Play(animationName);
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == animationName) {
                return State.Running;
            }
            return State.Success;
        }
    }
}
