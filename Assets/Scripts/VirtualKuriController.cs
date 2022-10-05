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
        float ForwardSpeed = 1f, BackwardSpeed = 0.8f, TurnSpeed = 1.5f, PointSpeed = 1f;
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
        float headYConstraintAngle = 50f;
        Transform _rightIKObject = null;
        Transform _leftIKObject = null;
        public Transform RightIKObject {
            get {
                if (_rightIKObject == null) {
                    SetUpIKArmObjects();
                }
                return _rightIKObject;
            }
        }
        public Transform LeftIKObject {
            get {
                if (_leftIKObject == null) {
                    SetUpIKArmObjects();
                }
                return _leftIKObject;
            }
        }

        public Animator ArmAnimator; // super flimsy

        public bool IsPointing = false;
        string onFrameAction = ""; // hacky quick way to pass the curaction from within actual actions to the update action of the virtual aciton taker
        public float timeSinceLastActionEndeded = 0;
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
        public override string TakeMovementAction(int option = -1) {
            if (option == -1) {
                int topOfRange = 3;
                if (MazeManager.instance.ContainsSolutionMaze() || MazeManager.instance.IsLocked) { // avoid going to misaligned pieces if not in building mode
                    topOfRange = 2;
                }
                option = Random.Range(0, topOfRange); // for some unforsaken reason the top end of the range on random.range is inclusive??? who made this decision? -> https://docs.unity3d.com/ScriptReference/Random.Range.html
            }
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
        public string MoveToUser() {
            onFrameAction = "MoveToUser";
            Vector3 newPos = GetPosWDistAway(TKTransformManager.Position, UserTransform.position, 1.1f);
            //StartCoroutine(LookAtAndGoToAtSpeed(UserTransform, newPos, ForwardSpeed));
            return onFrameAction;
        }
        public override void TurnTowardsUser() {
            // get distance from user
            onFrameAction = "TurnTowardsUser";
            Vector3 end = GetPosWDistAway(TKTransformManager.Position, UserTransform.position, Vector3.Distance(TKTransformManager.Position, UserTransform.position) - 0.1f);
            //StartCoroutine(LookAtAndGoToAtSpeed(UserTransform, end, ForwardSpeed));
        }
        public override string TakeISAAction() {
            onFrameAction = ExerciseManager.instance.GetCurExercise().GetComponent<ExerciseInformationSeekingActions>().DoISAAction();
            loggingManager.UpdateLogColumn(rISACol, onFrameAction);
            return onFrameAction;
        }
        public override string PointAtObj(Transform objectOfInterest, float time) {
            onFrameAction += actionSeperator + "PointAtObject: " + objectOfInterest.ToString();
            //StartCoroutine(PointAtObjectOverTime(objectOfInterest, time));
            return onFrameAction;
        }

        #endregion
        #region protected
        protected override bool UpdateCurrentActionString() {
            timeSinceLastActionEndeded = TutorKuriManager.instance.TimeLastActionEnded.TimeSince();
            string doingAnim = Anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            CurAction = onFrameAction;
            if (doingAnim != "neutral") {
                CurAction += actionSeperator + doingAnim;
            }
            if (kuriTextManager.IsTalking) {
                CurAction += actionSeperator + kuriTextManager.CurText;
            }
            onFrameAction = ""; // deals with movement actions, mega hacky
            return CurAction != ""; // note this is logged by the funciton that calls is
        }
        #endregion

        #region private
        private void SetUpIKArmObjects() {
            foreach (TargetIKObject tiko in GetComponentsInChildren<TargetIKObject>()) {
                if (tiko.IsRightArm) {
                    _rightIKObject = tiko.transform;
                }
                else {
                    _leftIKObject = tiko.transform;
                }
            }
        }
        private IEnumerator LookAtAndGoToAtSpeed(Transform objectToLookAt, Vector3 goalPosition, float speedinMS) {
            Assert.IsTrue(speedinMS > 0);
            Vector3 straightLine;
            Vector3 end;
            Bezier bezierCurve;
            CalculateBezierPath(goalPosition, out end, out bezierCurve, out straightLine);
            float origDist = Vector3.Distance(TKTransformManager.Position, end);

            float approxLength = bezierCurve.ApproximateTotalLength();
            float totalTime = approxLength / speedinMS;
            float t = 0;
            while (t < totalTime && !TKTransformManager.IsAtPosition(end)) {
                onFrameAction += actionSeperator + "LookAtAndGoToAtSpeed";
                t += Time.deltaTime;
                TKTransformManager.Position = bezierCurve.GetBezierPoint(t / totalTime);
                if (!TKTransformManager.IsAtPosition(end) && t < totalTime) {
                    RotateBodyAndHeadAlongPath(objectToLookAt, goalPosition);
                    yield return null;
                }
            }
            TKTransformManager.Position = end;
            if (origDist < Vector3.Distance(TKTransformManager.Position, end)) {
                CalculateBezierPath(goalPosition, out end, out bezierCurve, out straightLine); // recalc straightline from new spot
            }

            RotateBodyAndHeadAlongPath(objectToLookAt, end + straightLine);
        }
        private void CalculateBezierPath(Vector3 goalPosition, out Vector3 end, out Bezier bezierCurve, out Vector3 straightLine) {
            Vector3 start = TKTransformManager.Position;
            end = goalPosition;
            Vector3 tangent = (end - start).normalized;
            Vector3 normal = Vector3.Cross(tangent, Vector3.up).normalized;
            Vector3 controlPoint = start + tangent * 0.5f + normal * 0.5f;
            // get the ground plane and place all y values to the ground plane y
            Transform groundPlane = Pogp.GetGroundPlane();
            if (groundPlane != null) {
                start.y = groundPlane.position.y;
                end.y = groundPlane.position.y;
                controlPoint.y = groundPlane.position.y;
            }
            else { // else use kuri's y
                start.y = TKTransformManager.Position.y;
                end.y = TKTransformManager.Position.y;
                controlPoint.y = TKTransformManager.Position.y;
            }
            bezierCurve = new Bezier(Bezier.BezierType.Quadratic, new Vector3[] { start, controlPoint, end });
            //calculate finalRotation as rotating from the start to the goal
            straightLine = (goalPosition - start).normalized;
        }
        private string MoveToGoal() {
            onFrameAction = "GoingToGoal";
            Transform goalMPT = MazeManager.instance.GoalMazePiece.transform;
            MoveToMazePiece(goalMPT);
            return onFrameAction;
        }
        private string MoveToMisalignedPiece() {
            // should assert that the maze is not locked
            onFrameAction = "MoveToMisalignedPiece";
            Transform misalignedPieceT = MazeManager.instance.GetMisalignedPiece().transform;
            if (misalignedPieceT == null) {
                return MoveToUser(); // default to move to user if the goal pieces are all good
            }
            MoveToMazePiece(misalignedPieceT, true);
            return onFrameAction;
        }
        private void MoveToMazePiece(Transform mazePieceT, bool isMisaligned = false) { // nothing quite like hacky optional params
            Vector3 newPos = GetPosWDistAway(transform.position, mazePieceT.position, 1.2f);
            StartCoroutine(LookAtAndGoToAtSpeed(mazePieceT, newPos, ForwardSpeed));
            PointAtObj(mazePieceT, 5f);
            if (isMisaligned) {
                string mpTypeName = mazePieceT.GetComponent<MazePiece>().MyMPType.Name;
                KuriTextManager.instance.Addline($"You might need this {mpTypeName} piece of the maze.");
            }
        }
        private Vector3 GetPosWDistAway(Vector3 start, Vector3 end, float distAway) {
            Vector3 dir = end - start;
            return start + dir.normalized * (dir.magnitude - distAway);
        }
        private string MoveToBKMazePiece() {
            onFrameAction = "MoveToBKMazePiece";
            Transform BKMakePieceT = MazeManager.instance.BKMazePiece.transform;
            MoveToMazePiece(BKMakePieceT);
            return onFrameAction;
        }
        private void RotateBodyAndHeadAlongPath(Transform objectToLookAt, Vector3 goalPosition) {
            Vector3 goalDir = (goalPosition - TKTransformManager.Position).normalized;
            Vector3 goalRot = Quaternion.LookRotation(goalDir, Vector3.up).eulerAngles;
            Vector3 bodyRot = TKTransformManager.BodyRotation.eulerAngles;
            bodyRot.y = goalRot.y;
            TKTransformManager.BodyRotation = Quaternion.Euler(bodyRot);
            if (objectToLookAt != null) {
                TKTransformManager.HeadRotation = Quaternion.LookRotation(objectToLookAt.position - TKTransformManager.Position, Vector3.up);
                // check head rotation is not outside of constraint relative to body rotation                
                Vector3 headRot = TKTransformManager.HeadRotation.eulerAngles.Norm180Minus180();
                // get head rotation relative to body rotation
                headRot -= TKTransformManager.BodyRotation.eulerAngles.Norm180Minus180();
                // check if head rotation is outside of constraint
                if (headRot.y > headYConstraintAngle) {
                    headRot.y = headYConstraintAngle;
                }
                else if (headRot.x < -headYConstraintAngle) {
                    headRot.x = -headYConstraintAngle;
                }
                // set head rotation to be relative to body rotation
                headRot += TKTransformManager.BodyRotation.eulerAngles.Norm180Minus180();
                TKTransformManager.HeadRotation = Quaternion.Euler(headRot);
            }
        }

        private IEnumerator PointAtObjectOverTime(Transform objectToPointAt, float time) {
            if (IsPointing) {
                IsPointing = false; // stop other pointing routine
                yield return null;
            }
            IsPointing = true;
            ArmAnimator.enabled = false;
            Vector3 origLocalPositionR = RightIKObject.localPosition;
            Vector3 origLocalPositionL = LeftIKObject.localPosition;
            Vector3 startRPos = RightIKObject.position;
            Vector3 startLPos = LeftIKObject.position;

            float t = 0;
            float approxLengthR = Vector3.Distance(RightIKObject.position, objectToPointAt.position);
            float approxLengthL = Vector3.Distance(LeftIKObject.position, objectToPointAt.position);
            float timeToR = approxLengthR / PointSpeed;
            float timeToL = approxLengthL / PointSpeed;
            while (t < time) {
                onFrameAction += actionSeperator + "Pointing";
                t += Time.deltaTime;
                RightIKObject.position = Vector3.Lerp(startRPos, objectToPointAt.position, Mathf.Min(t / timeToR, 1));
                LeftIKObject.position = Vector3.Lerp(startLPos, objectToPointAt.position, Mathf.Min(t / timeToL, 1));
                yield return null;
            }
            RightIKObject.localPosition = origLocalPositionR;
            LeftIKObject.localPosition = origLocalPositionL;
            IsPointing = false;
            ArmAnimator.enabled = true;
        }

        public override string MoveToObj(Transform obj) {
            throw new System.NotImplementedException();
        }

        protected override void Init() {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
