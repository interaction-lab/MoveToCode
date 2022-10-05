namespace MoveToCode {
    public class TurnOffAllAnimators : TurnOffAnimator {
        protected override void TurnOffAnim() {
            BodyAnimator.enabled = false;
            ArmAnimator.enabled = false;
        }
    }
}
