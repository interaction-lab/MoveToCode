namespace MoveToCode {
    public class LogOnAnimationEnded : LogActionEnded {
        protected override void SetActionName() {
            actionName = string.Join(Separator, EventNames.OnDoAnimation, blackboard.emotion.ToString());
        }
    }
}
