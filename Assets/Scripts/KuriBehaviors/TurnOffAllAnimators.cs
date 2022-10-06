namespace MoveToCode {
    public class TurnOffAllAnimators : TurnOffAnimator {
        protected override void TurnOffAnim() {
            if (--blackboard.BodyAnimatorSemaphoreCount == 0) {
                BodyAnimator.enabled = false;
            }
            if (--blackboard.ArmAnimatorSemaphoreCount == 0) {
                ArmAnimator.enabled = false;
            }
        }
    }
}
