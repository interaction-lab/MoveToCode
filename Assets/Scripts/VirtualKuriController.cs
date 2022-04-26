using UnityEngine;
using System.Collections;
using UnityMovementAI;

namespace MoveToCode {
    public class VirtualKuriController : KuriController {
        #region members
        TutorKuriTransformManager tktm;
        TutorKuriTransformManager TKTransformManager {
            get {
                if (tktm == null) {
                    tktm = transform.parent.GetComponent<TutorKuriTransformManager>(); // flimsy and should get from the TutorKuriManager but oh well
                }
                return tktm;
            }
        }
        public float ForwardSpeed, BackwardSpeed, TurnSpeed;
        public Transform goalTransform;
        Animator anim;
        Animator Anim {
            get {
                if (anim == null) {
                    anim = GetComponent<Animator>();
                }
                return anim;
            }
        }

        PlaceOnGroundPlane pogp;
        PlaceOnGroundPlane Pogp {
            get {
                if (pogp == null) {
                    pogp = GetComponent<PlaceOnGroundPlane>();
                }
                return pogp;
            }
        }

        #endregion
        #region unity
        #endregion
        #region public
        public override string DoAnimationAction(EMOTIONS e) {
            string action = e.ToString();
            Anim.SetTrigger(action);
            return action;
        }

        public override string DoRandomNegativeAction() {
            return DoAnimationAction(NegativeEmotions[Random.Range(0, NegativeEmotions.Length)]);
        }

        public override string DoRandomPositiveAction() {
            return DoAnimationAction(PositiveEmotions[Random.Range(0, NegativeEmotions.Length)]);
        }

        public override string TakeMovementAction() {
            //move to user
            GoToUser();
            return "moving";
        }

        public void GoToUser() {
            CurAction = "GoingToUser";
            StartCoroutine(GoToPosition(Camera.main.transform, goalTransform.position, 1.5f));
        }

        public IEnumerator GoToPosition(Transform objectToLookAt, Vector3 goalPosition, float speedinMS) {
            // if objectToLookAt
            Vector3 start = TKTransformManager.Position;
            Vector3 end = goalPosition;
            Vector3 tangent = (end - start).normalized;
            Vector3 normal = Vector3.Cross(tangent, Vector3.up).normalized;
            Vector3 controlPoint = start + tangent * 0.5f + normal * 0.5f;
            // get the ground plane and place all y values to the ground plane y
            Transform groundPlane = Pogp.GetGroundPlane();
            if (groundPlane != null) {
                end.y = groundPlane.position.y;
                controlPoint.y = groundPlane.position.y;
            }
            Bezier bezierCurve = new Bezier(Bezier.BezierType.Quadratic, new Vector3[] { start, controlPoint, end });
            float approxLength = bezierCurve.ApproximateTotalLength();
            float time = approxLength / speedinMS;
            float t = 0;
            while (t < 1 && !TKTransformManager.IsAtPosition(end)) {
                t += (Time.deltaTime / time);
                TKTransformManager.Position = bezierCurve.GetBezierPoint(t);
                // make TKTransformManager body rotation face the goal but rotate only about the y axis
                if (t < 1) {
                    Vector3 goalDir = (goalPosition - TKTransformManager.Position).normalized;
                    Vector3 goalRot = Quaternion.LookRotation(goalDir, Vector3.up).eulerAngles;
                    Vector3 bodyRot = TKTransformManager.BodyRotation.eulerAngles;
                    bodyRot.y = goalRot.y;
                    TKTransformManager.BodyRotation = Quaternion.Euler(bodyRot);
                    if (objectToLookAt != null) {
                        TKTransformManager.HeadRotation = Quaternion.LookRotation(objectToLookAt.position - TKTransformManager.Position, Vector3.up);
                    }
                }
                yield return null;
            }
            TKTransformManager.Position = end;
        }

        public override void TurnTowardsUser() {
            throw new System.NotImplementedException();
        }
        #endregion
        #region protected
        protected override bool UpdateCurrentActionString() {
            string doingAnim = Anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            CurAction = "";
            if (doingAnim != "neutral") {
                CurAction = actionSeperator + doingAnim;
            }
            if (kuriTextManager.IsTalking) {
                CurAction += actionSeperator + kuriTextManager.CurTextCommand.ToString();
            }

            // TODO: Movement when doing the movement actions
            return CurAction != "";
        }
        #endregion
    }
}
