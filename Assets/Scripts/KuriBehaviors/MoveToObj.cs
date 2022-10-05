using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class MoveToObj : ActionNode {
        TutorKuriTransformManager kuriTransformManager;
        Transform goalObj;
        float distThreshold = 0.1f, speedinMS = 1f;
        float approxLength, totalTime;
        float positionAlongCurve = 0f;
        Bezier bezierCurve;
        Vector3 objPosWhenBezWasCalculated;
        protected override void OnStart() {
            goalObj = blackboard.objToMoveTo;
            kuriTransformManager = TutorKuriTransformManager.instance;
            // calculate a Bezier curve to move along
            CalcBezCurve();
        }

        private void CalcBezCurve() {
            bezierCurve = new Bezier(
                Bezier.BezierType.Linear,
                new Vector3[]{
                    kuriTransformManager.Position,
                    goalObj.position});
            objPosWhenBezWasCalculated = goalObj.position;
            approxLength = bezierCurve.ApproximateTotalLength();
            totalTime = approxLength / speedinMS;
        }
        
        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            // if the object has moved, recalculate the curve
            if (objPosWhenBezWasCalculated != goalObj.position) {
                CalcBezCurve();
            }
            // check if they are close
            if (Vector3.Distance(kuriTransformManager.Position, goalObj.position) < distThreshold) {
                return State.Success;
            }

            // move toward goal using bezier curve
            positionAlongCurve += Time.deltaTime / totalTime;
            kuriTransformManager.Position = bezierCurve.GetBezierPoint(positionAlongCurve);
            return State.Running;
        }
    }
}
