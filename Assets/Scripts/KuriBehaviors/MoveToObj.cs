using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class MoveToObj : ActionNode {
        TutorKuriTransformManager kuriTransformManager;
        Transform goalObj;
        float distThreshold = 0.1f, speedinMS = 0.5f, bezTimeThreshold = 0.95f;
        float approxLength, totalTime;
        float positionAlongCurve = 0f;
        Bezier bezierCurve;
        Vector3 objPosWhenBezWasCalculated, start, end;
        protected override void OnStart() {
            goalObj = blackboard.objToMoveTo;
            kuriTransformManager = TutorKuriTransformManager.instance;
            // calculate a Bezier curve to move along
            CalcBezCurve();
        }

        private void CalcBezCurve() {
            start = kuriTransformManager.Position;
            end = goalObj.position;
            end.y = kuriTransformManager.GroundYCord;

            Vector3 lineVec = end - start;

            // calculate the length of the line
            float approxLineLength = lineVec.magnitude;


            Vector3 tangent = (lineVec);
            Vector3 normal = Vector3.Cross(tangent, Vector3.up);
            Vector3 controlPoint = start + tangent * 0.5f + normal * 0.5f;

            bezierCurve = new Bezier(
                Bezier.BezierType.Quadratic,
                new Vector3[3] { start, controlPoint, end });

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
            if (Vector3.Distance(kuriTransformManager.Position, end) < distThreshold) {
                return State.Success;
            }

            // move toward goal using bezier curve
            positionAlongCurve += Time.deltaTime / totalTime;
            kuriTransformManager.Position = bezierCurve.GetBezierPoint(positionAlongCurve);

            if (positionAlongCurve >= bezTimeThreshold) {
                return State.Success; // return if they are on top of the end
            }
            return State.Running;
        }
    }
}
