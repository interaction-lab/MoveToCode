using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public abstract class TurnOffAnimator : ActionNode {
        Animator bAnim, aAnim;
        protected Animator BodyAnimator {
            get {
                if (bAnim == null) {
                    bAnim = context.mainAnimator;
                }
                return bAnim;
            }
        }
        protected Animator ArmAnimator {
            get {
                if(aAnim == null) {
                    aAnim = context.armAnimator;
                }
                return aAnim;
            }
        }
        protected override void OnStart() {
            TurnOffAnim();
        }
        
        protected abstract void TurnOffAnim();

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            return State.Success;
        }
    }
}
