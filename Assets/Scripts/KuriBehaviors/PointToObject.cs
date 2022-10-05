using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class PointToObject : ActionNode {
        KuriController KController;
        Transform objToPointTo, ikTransform;
        Vector3 startPos, endPos;
        private float speed;
        private float maxArmVectorLength = 1; // idk just guessing here lol

        protected override void OnStart() {
            KController = context.KController;
            objToPointTo = blackboard.objToPointTo;
            ikTransform = KController.IkObjRight.transform;
            startPos = ikTransform.position;
            endPos = objToPointTo.position;
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            ikTransform.position = objToPointTo.position;

            return State.Success;
        }
    }
}
