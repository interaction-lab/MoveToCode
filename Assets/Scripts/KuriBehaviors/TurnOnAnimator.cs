using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class TurnOnAnimator : ActionNode {
        Animator anim;
        Animator ThisAnim {
            get {
                if (anim == null) {
                    anim = context.animator;
                }
                return anim;
            }
        }
        protected override void OnStart() {
            ThisAnim.enabled = true;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            return State.Success;
        }
    }
}
