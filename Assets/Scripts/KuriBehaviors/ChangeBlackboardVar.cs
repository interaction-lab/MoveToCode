using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class ChangeBlackboardVar : ActionNode {
        #region members
        public bool TurnToUser = false;
        public bool LookAtUser = false;
        #endregion
        #region overrides
        protected override void OnStart() {
            // set all blackboard vars
            if (TurnToUser) {
                blackboard.objToTurnTo = Camera.main.transform;
            }
            if (LookAtUser) {
                blackboard.objToLookAt = Camera.main.transform;
            }
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            return State.Success;
        }
        #endregion
        #region helpers
        #endregion
    }
}
