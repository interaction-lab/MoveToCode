namespace MoveToCode {
    public class TurnOnAllAnimators : TurnOnAnimator {
        protected override void TurnOnAnim() {
            BodyAnimator.enabled = true;
            ArmAnimator.enabled = true;
        }
    }
}
