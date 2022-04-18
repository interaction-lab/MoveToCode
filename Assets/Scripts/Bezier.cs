using UnityEngine;

namespace MoveToCode {
    public class Bezier {
        #region members
        public enum BezierType {
            None,
            Linear,
            Quadratic,
            Cubic
        }
        BezierType bezierType;
        public BezierType MyBezierType {
            get {
                if (bezierType == BezierType.None) {
                    bezierType = BezierType.Linear;
                }
                return bezierType;
            }
            set {
                bezierType = value;
            }
        }

        public Vector3[] controlPoints;
        public Vector3[] MyControlPoints {
            get {
                if (controlPoints == null) {
                    controlPoints = new Vector3[3];
                }
                return controlPoints;
            }
            set {
                controlPoints = value;
            }
        }

        Vector3[] bezierPoints;
        public Vector3[] MyBezierPoints {
            get {
                if (bezierPoints == null) {
                    bezierPoints = new Vector3[3];
                }
                return bezierPoints;
            }
            set {
                bezierPoints = value;
            }
        }


        #endregion

        #region public
        public Bezier(BezierType bezierType, Vector3[] controlPoints) {
            MyBezierType = bezierType;
            MyControlPoints = controlPoints;
        }

        public Vector3[] GetBezierPoints(int numPoints) {
            MyBezierPoints = new Vector3[numPoints];
            float t = 0;
            float step = 1f / (numPoints - 1);
            for (int i = 0; i < numPoints; i++) {
                MyBezierPoints[i] = GetBezierPoint(t);
                t += step;
            }
            return MyBezierPoints;
        }

        public Vector3 GetBezierPoint(float t) {
            switch (MyBezierType) {
                case BezierType.Linear:
                    return LinearBezier(t);
                case BezierType.Quadratic:

                    return QuadraticBezier(t);
                case BezierType.Cubic:
                    return CubicBezier(t);
                default:
                    return Vector3.zero;
            }
        }

        #endregion

        #region private
        Vector3 LinearBezier(float t) {
            return (1 - t) * MyControlPoints[0] + t * MyControlPoints[1];
        }

        Vector3 QuadraticBezier(float t) {
            return (1 - t) * (1 - t) * MyControlPoints[0] + 2 * (1 - t) * t * MyControlPoints[1] + t * t * MyControlPoints[2];
        }

        Vector3 CubicBezier(float t) {
            return (1 - t) * (1 - t) * (1 - t) * MyControlPoints[0] + 3 * (1 - t) * (1 - t) * t * MyControlPoints[1] + 3 * (1 - t) * t * t * MyControlPoints[2] + t * t * t * MyControlPoints[3];
        }

        #endregion

    }
}
