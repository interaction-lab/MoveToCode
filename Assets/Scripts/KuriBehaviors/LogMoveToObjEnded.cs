using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode{
    public class LogMoveToObjEnded : LogActionEnded {
        protected override void SetActionName() {
            actionName = string.Join(Separator,EventNames.OnMoveToObj, blackboard.objToMoveTo.name);
        }
    }
}
