namespace MoveToCode {
    public class LogOnAnimationStarted : LogActionStarted {
        protected override void SetActionName() {
            actionName = string.Join(Separator, EventNames.OnDoAnimation, blackboard.emotion.ToString());
        }
    }
}
