using UnityEngine;
using System.Collections;
using UnityMovementAI;
using UnityEngine.Assertions;

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
        public float ForwardSpeed = 1.5f, BackwardSpeed = 1f, TurnSpeed = 1.5f;
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

        Transform _userTransform;
        Transform UserTransform {
            get {
                if (_userTransform == null) {
                    _userTransform = Camera.main.transform;
                }
                return _userTransform;
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
            // options are
            // 1. move close to the BKMazePiece
            // 2. move close to the user
            // 3. move close to the goal
            // 4. move close to a piece that seems slightly misaligned

            // Get random option from the above
            int option = Random.Range(0, 4);
            Debug.Log(option);
            string action = "";
            switch (option) {
                case 0:
                    action = MoveToBKMazePiece();
                    break;
                case 1:
                    action = MoveToUser();
                    break;
                case 2:
                    action = MoveToGoal();
                    break;
                case 3:
                    action = MoveToMisalignedPiece();
                    break;
            }
            return action;
        }

        private Vector3 GetPosWDistAway(Vector3 start, Vector3 end, float distAway) {
            Vector3 dir = end - start;
            return start + dir.normalized * (dir.magnitude - distAway);
        }
        private string MoveToBKMazePiece() {
            CurAction = "MoveToBKMazePiece";
            Transform BKMakePieceT = MazeManager.instance.BKMazePiece.transform;
            MoveToMazePiece(BKMakePieceT);
            return CurAction;
        }
        private string MoveToUser() {
            CurAction = "MoveToUser";
            Vector3 newPos = GetPosWDistAway(TKTransformManager.Position, UserTransform.position, 0.5f);
            StartCoroutine(LookAtAndGoToAtSpeed(UserTransform, newPos, ForwardSpeed));
            return CurAction;
        }

        private string MoveToGoal() {
            CurAction = "GoingToGoal";
            Transform goalMPT = MazeManager.instance.GoalMazePiece.transform;
            MoveToMazePiece(goalMPT);
            return CurAction;
        }

        private string MoveToMisalignedPiece() {
            CurAction = "MoveToMisalignedPiece";
            Transform misalignedPieceT = MazeManager.instance.GetMisalignedPiece().transform;
            if(misalignedPieceT == null){
                return MoveToUser(); // default to move to user if the goal pieces are all good
            }
            MoveToMazePiece(misalignedPieceT);
            return CurAction;
        }

        private void MoveToMazePiece(Transform mazePieceT) {
            Vector3 newPos = GetPosWDistAway(transform.position, mazePieceT.position, 0.5f);
            StartCoroutine(LookAtAndGoToAtSpeed(mazePieceT, newPos, ForwardSpeed));
        }
        private IEnumerator LookAtAndGoToAtSpeed(Transform objectToLookAt, Vector3 goalPosition, float speedinMS) {
            Assert.IsTrue(speedinMS > 0);

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
            float totalTime = approxLength / speedinMS;
            float t = 0;
            while (t < totalTime && !TKTransformManager.IsAtPosition(end)) {
                t += Time.deltaTime;
                TKTransformManager.Position = bezierCurve.GetBezierPoint(t/totalTime);
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
            Debug.Log("done");
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
