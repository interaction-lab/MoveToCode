namespace MoveToCode {
    public class TurnOnAllAnimators : TurnOnAnimator {
        protected override void TurnOnAnim() {
            if(++blackboard.BodyAnimatorSemaphoreCount == 1) {
                BodyAnimator.enabled = true;
            }
            if(++blackboard.ArmAnimatorSemaphoreCount == 1) {
                ArmAnimator.enabled = true;
            }
        }
    }
}
