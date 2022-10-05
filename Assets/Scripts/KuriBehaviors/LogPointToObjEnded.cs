using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode{
    public class LogPointToObjEnded : LogActionEnded {
        protected override void SetActionName() {
            actionName = LogPointToObjStarted.ActionName + blackboard.objToPointTo.name;
        }
    }
}
