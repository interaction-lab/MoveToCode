using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class WaitUntilAnimatorsReleased : ActionNode {
        public float timeToWait = 5f;
        float timeWaited = 0f;
        public bool WaitForBodyAnim = true;
        public bool WaitForArmAnim = true;
        public int stackOnMySide = 0; // will allow a queue to happen on one side
        protected override void OnStart() {
            timeWaited = 0f;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            if (timeWaited > timeToWait) {
                return State.Failure;
            }
            if (IsStackZero()) { // means we only want to grab if the animator is fully released aka no one is using it
                bool res = true;
                if (WaitForBodyAnim) {
                    res &= IsBodyZero();
                }
                if (WaitForArmAnim) {
                    res &= IsArmZero();
                }
                if (res) {
                    return State.Success;
                }
            }
            else if (IsStackPositive()) {
                bool res = true;
                if (WaitForBodyAnim) {
                    res &= IsBodyPositive();
                }
                if (WaitForArmAnim) {
                    res &= IsArmPositive();
                }
                if (res) {
                    return State.Success;
                }
            }
            else if (IsStackNegative()) {
                bool res = true;
                if (WaitForBodyAnim) {
                    res &= IsBodyNegative();
                }
                if (WaitForArmAnim) {
                    res &= IsArmNegative();
                }
                if (res) {
                    return State.Success;
                }
            }
            timeWaited += Time.deltaTime;
            return State.Running;
        }

        #region helpers
        bool IsBodyZero() {
            return blackboard.BodyAnimatorSemaphoreCount == 0;
        }
        bool IsArmZero() {
            return blackboard.ArmAnimatorSemaphoreCount == 0;
        }
        bool IsBodyPositive() {
            return blackboard.BodyAnimatorSemaphoreCount >= 0;
        }
        bool IsArmPositive() {
            return blackboard.ArmAnimatorSemaphoreCount >= 0;
        }
        bool IsBodyNegative() {
            return blackboard.BodyAnimatorSemaphoreCount <= 0;
        }
        bool IsArmNegative() {
            return blackboard.ArmAnimatorSemaphoreCount <= 0;
        }
        bool IsStackNegative() {
            return stackOnMySide < 0;
        }
        bool IsStackPositive() {
            return stackOnMySide > 0;
        }

        bool IsStackZero() {
            return stackOnMySide == 0;
        }
        #endregion
    }
}
