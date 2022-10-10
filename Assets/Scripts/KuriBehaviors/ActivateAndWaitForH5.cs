using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class ActivateAndWaitForH5 : ActionNode {
        #region members
        float maxTimeToWait = 10f, startTime;
        KuriArms kuriArms;
        bool handHit = false, initialized = false;
        #endregion
        #region overrides
        protected override void OnStart() {
            handHit = false;
            startTime = Time.time;
            kuriArms = context.kuriArms;
            kuriArms.RightIKTarget.OnHitHand.AddListener(OnHitHand);
            blackboard.ArmAnimatorSemaphoreCount -= 1;
            initialized = true;
        }

        protected override void OnStop() {
            if (initialized) {
                kuriArms.RightIKTarget.OnHitHand.RemoveListener(OnHitHand);
                blackboard.emotion = KuriController.EMOTIONS.h5_end;
                blackboard.ArmAnimatorSemaphoreCount += 1;
            }
        }

        protected override State OnUpdate() {
            if (handHit) {
                return State.Success;
            }
            if (Time.time - startTime > maxTimeToWait) {
                return State.Failure;
            }
            return State.Running;
        }
        #endregion
        #region helpers
        void OnHitHand() {
            handHit = true;
        }
        #endregion
    }
}
