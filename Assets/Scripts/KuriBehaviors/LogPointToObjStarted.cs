using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode{
    public class LogPointToObjStarted : LogActionStarted {
        public static string ActionName = "PointToObj ";
        protected override void SetActionName() {
            actionName = ActionName + blackboard.objToPointTo.name;
        }
    }
}
