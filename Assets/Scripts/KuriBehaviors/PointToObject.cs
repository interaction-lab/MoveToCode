using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class PointToObject : ActionNode {
        VirtualKuriController virtualKuriController;
        Transform objToPointTo, ikTransform;
        
        protected override void OnStart() {
            virtualKuriController = context.virtualKuriController;
            objToPointTo = blackboard.objToPointTo;
            // get the right and left shoulder transforms
            // see which is closer to the object
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            
            return State.Success;
        }
    }
}
