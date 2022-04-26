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
        Animator anim;
        Animator Anim {
            get {
                if (anim == null) {
                    anim = GetComponent<Animator>();
                }
                return anim;
            }
        }

        MakeTransformLookAtUser mtlau;
        MakeTransformLookAtUser Mtlau {
            get {
                if (mtlau == null) {
                    mtlau = GetComponent<MakeTransformLookAtUser>();
                }
                return mtlau;
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
            StartCoroutine(GoToPosition(Camera.main.transform.position));
        }

        public IEnumerator GoToPosition(Vector3 goal) {
            // use a Bezier curve to get there in a smooth motion (not just straight line), making sure kuri is always facing tanget the curve
            Vector3 start = TKTransformManager.Position;
            Vector3 end = goal;
            Vector3 tangent = (end - start).normalized;
            Vector3 normal = Vector3.Cross(tangent, Vector3.up).normalized;
            Vector3 controlPoint = start + tangent * 0.5f + normal * 0.5f;
            // get the ground plane and place all y values to the ground plane y
            Transform groundPlane = Pogp.GetGroundPlane();
            if(groundPlane != null) {
                end.y = groundPlane.position.y;
                controlPoint.y = groundPlane.position.y;
            }
            Bezier bezierCurve = new Bezier(Bezier.BezierType.Quadratic,new Vector3[]{start, controlPoint, end});
            float t = 0;
            while (t < 1) {
                t += Time.deltaTime;
                TKTransformManager.Position = bezierCurve.GetBezierPoint(t);
                // make sure kuri is always facing the tangent
                // I want the body to look at the tangent, but I don't know how to do that
                // and then I want the head to look at the user

                
                TKTransformManager.transform.LookAt(TKTransformManager.Position + tangent);
                yield return null;
            }
            TKTransformManager.Position = end;
        }

        public override void TurnTowardsUser() {
            Mtlau.LookAtUser();
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
