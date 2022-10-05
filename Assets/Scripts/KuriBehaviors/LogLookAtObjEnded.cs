using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class LogLookAtObjEnded : LogActionEnded {
        protected override void SetActionName() {
            actionName = string.Join(Separator, EventNames.OnLookAtObj, blackboard.objToLookAt.name);
        }
    }
}
