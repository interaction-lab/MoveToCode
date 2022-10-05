using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public abstract class LogActionStarted : ActionNode {
        public string actionName;
        public static string ActionStartedCol = "KTActionStarted";
        LoggingManager lm;
        LoggingManager LoggingManagerInstance {
            get {
                if (lm == null) {
                    lm = LoggingManager.instance;
                }
                return lm;
            }
        }

        protected override void OnStart() {
            SetActionName();
            if (actionName == "") {
                Debug.LogWarning("No action name given to LogActionStarted");
            }
            if (!LoggingManagerInstance.GetColumnLookUp().ContainsKey(ActionStartedCol)) {
                LoggingManagerInstance.AddLogColumn(ActionStartedCol, "");
            }
        }

        protected abstract void SetActionName();

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            // get the current log string
            string logString = LoggingManagerInstance.GetValueInRowAt(ActionStartedCol);
            if (logString == "") {
                LoggingManagerInstance.UpdateLogColumn(ActionStartedCol, actionName);
            }
            else {
                LoggingManagerInstance.UpdateLogColumn(ActionStartedCol, string.Join(Node.actionSeparator, logString, actionName));
            }
            return State.Success;
        }
    }
}
