namespace MoveToCode{
    public class LogLookAtObjStarted : LogActionStarted {
        protected override void SetActionName() {
            actionName = string.Join(Separator, EventNames.OnLookAtObj, blackboard.objToLookAt.name);
        }
    }
}
