using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace MoveToCode {
    public class LoggableMoveToObj : LoggableBehPrimitive {
        #region members
        TutorKuriTransformManager kuriTransformManager;
        Transform goalObj;
        float distThreshold = 0.5f, speedinMS = 0.5f, bezTimeThreshold = 0.95f, controlPointScaler = 0.1f;
        float approxLength, totalTime;
        float positionAlongCurve = 0f;
        Bezier bezierCurve;
        Vector3 objPosWhenBezWasCalculated, start, end;
        public static LoggableMoveToObj CurLoggableMoveToObj = null;
        #endregion
        #region overrides
        protected override void BehCleanUp() {
            if (CurLoggableMoveToObj == this) {
                CurLoggableMoveToObj = null;
            }
            controlPointScaler *= -1; // makes the turns alternate left and right
        }

        protected override void BehSetUp() {
            CurLoggableMoveToObj = this;
            speedinMS = blackboard.bodySpeed;
            kuriTransformManager = TutorKuriTransformManager.instance;
            CalcBezCurve();
        }

        protected override State OnUpdate() {
            if (CurLoggableMoveToObj != this) {
                return State.Success; // quietly finish
            }
            // if the object has moved, recalculate the curve
            if (NotCloseBabyyy(objPosWhenBezWasCalculated, goalObj.position)) {
                Debug.Log("new goal position");
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

        protected override void SetAnimatorSemaphoreCount() {
            AddToBodyAnimatorSemaphore = 1;
        }

        protected override void SetLogActionName() {
            actionName = string.Join(Separator,
                EventNames.OnMoveToObj,
                blackboard.objToMoveTo.name);
        }
        #endregion
        #region helpers
        private bool NotCloseBabyyy(Vector3 a, Vector3 b) {
            return Vector3.Distance(a, b) > 0.1f;
        }
        private void CalcBezCurve() {
            goalObj = blackboard.objToMoveTo;
            if (goalObj == null) {
                Debug.LogError("LoggableMoveToObj: goalObj is null");
                KuriTextManager.instance.Addline("null obj");
                return;
            }
            KuriTextManager.instance.Addline(goalObj.position.ToString() + ", " + goalObj.gameObject.name);

            if (goalObj == PlayerTransformManager.instance.OriginT) {
                distThreshold = 0.8f;
            }
            else {
                distThreshold = 0.4f;
            }
            positionAlongCurve = 0f;

            start = kuriTransformManager.Position;
            end = goalObj.position;
            end.y = kuriTransformManager.GroundYCord;

            Vector3 lineVec = end - start;
            Vector3 tangent = (lineVec);
            Vector3 normal = Vector3.Cross(tangent, Vector3.up);
            Vector3 controlPoint = start + tangent * controlPointScaler + normal * controlPointScaler;

            bezierCurve = new Bezier(
                Bezier.BezierType.Quadratic,
                new Vector3[3] { start, controlPoint, end });

            objPosWhenBezWasCalculated = goalObj.position;
            approxLength = bezierCurve.ApproximateTotalLength();
            totalTime = approxLength / speedinMS;
        }
        #endregion
    }
}
