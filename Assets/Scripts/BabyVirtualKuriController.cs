using System.Collections;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

namespace MoveToCode {
    public class BabyVirtualKuriController : MonoBehaviour {

        #region members
        public string CurMovementAction = "";
        public static string babyKuriMovementActionCol = "babyKuriMovementAction";
        private float moveSpeed = 1f, turnSpeed = 50f;
        public AnimationCurve speedCurve;
        public MeshRenderer bodyPlateRend;

        BabyKuriManager bkm;
        BabyKuriManager babyKuriManager {
            get {
                if (bkm == null) {
                    bkm = BabyKuriManager.instance;
                }
                return bkm;
            }
        }

        private BabyKuriTransformManager _bktransformmanager;
        public BabyKuriTransformManager BKTransformManager {
            get {
                if (_bktransformmanager == null) {
                    _bktransformmanager = transform.parent.GetComponent<BabyKuriTransformManager>();
                }
                return _bktransformmanager;
            }
        }

        public static string MoveLogString { get; } = "Moving ";
        public static string TurnLogString { get; } = "Turning ";

        private Regex moveRe = new Regex(@"^" + MoveLogString);
        private Regex turnRe = new Regex(@"^" + TurnLogString);
        public bool IsMoving {
            get {
                return (moveRe.IsMatch(CurMovementAction) || turnRe.IsMatch(CurMovementAction)) && !interpreter.IsInResetState();
            }
        }
        Interpreter _interpreter;
        Interpreter interpreter {
            get {
                if (_interpreter == null) {
                    _interpreter = Interpreter.instance;
                }
                return _interpreter;
            }
        }

        /// <summary>
        /// Type is the type of movement
        /// float is the amount of movement (degrees for turns, dist in meters for moving)
        /// </summary>
        Queue<KeyValuePair<Type, float>> MoveQueue { get; set; } = new Queue<KeyValuePair<Type, float>>();

        #endregion

        #region unity
        private void Awake() {
            LoggingManager.instance.AddLogColumn(babyKuriMovementActionCol, "");
        }
        private void FixedUpdate() {
            LoggingManager.instance.UpdateLogColumn(babyKuriMovementActionCol, CurMovementAction);
        }
        #endregion

        #region public
        public void MoveOverTime(float dist) {
            MoveQueue.Enqueue(new KeyValuePair<Type, float>(typeof(CodeBlockEnums.Move), dist));
            StartNextMovement();
        }

        public void TurnOverTime(float degrees) {
            MoveQueue.Enqueue(new KeyValuePair<Type, float>(typeof(CodeBlockEnums.Turn), degrees));
            StartNextMovement();
        }

        public void ResetToOrigState() {
            BKTransformManager.ResetToOriginalState();
        }

        public void SetColor(Color color) {
            bodyPlateRend.material.color = color;
        }

        #endregion

        #region private
        private void StartNextMovement() {
            if (!MoveQueue.Empty() && !IsMoving) {
                KeyValuePair<Type, float> p = MoveQueue.Dequeue();
                if (p.Key == typeof(CodeBlockEnums.Move)) {
                    StartCoroutine(GoToPosition(BKTransformManager.KuriPos + BKTransformManager.Forward * p.Value, p.Value > 0));
                }
                else if (p.Key == typeof(CodeBlockEnums.Turn)) {
                    StartCoroutine(TurnToAngle(Quaternion.Euler(BKTransformManager.KuriRot.eulerAngles + BKTransformManager.Up * p.Value), p.Value > 0));
                }
            }
        }


        private float goalDistDelta = 0.02f;
        public IEnumerator GoToPosition(Vector3 goal, bool forward) {
            if (IsMoving) {
                throw new InvalidOperationException("Baby Kuri already moving, check moveQueue queueing code");
            }
            CurMovementAction = MoveLogString + (forward ? "FORWARD" : "BACKWARD") + " to " + goal.ToString();
            float totalDist = Vector3.Distance(BKTransformManager.KuriPos, goal);
            float curDist = totalDist;
            while (curDist > goalDistDelta && IsMoving) {
                Vector3 dir = (goal - BKTransformManager.KuriPos).normalized;
                float curSpeed = moveSpeed * speedCurve.Evaluate(curDist / totalDist);
                BKTransformManager.KuriPos = BKTransformManager.KuriPos + dir * curSpeed * Time.deltaTime;
                curDist = Vector3.Distance(BKTransformManager.KuriPos, goal);
                yield return null;
            }
            if (IsMoving) {
                BKTransformManager.KuriPos = goal;
            }
            ResetCurMovementAction();
            StartNextMovement();
        }

        private float goalDegreeDelta = 3f;
        public IEnumerator TurnToAngle(Quaternion goal, bool right) {
            if (IsMoving) {
                throw new InvalidOperationException("Baby Kuri already moving, check moveQueue queueing code");
            }
            CurMovementAction = MoveLogString + (right ? "RIGHT" : "LEFT") + " to " + goal.ToString();
            float totalDist = Quaternion.Angle(BKTransformManager.KuriRot, goal);
            float curDist = totalDist;
            while (Mathf.Abs(curDist) > goalDegreeDelta && IsMoving) {
                float curSpeed = turnSpeed * speedCurve.Evaluate(curDist / totalDist);
                BKTransformManager.KuriRot = Quaternion.Euler(BKTransformManager.KuriRot.eulerAngles + BKTransformManager.Up * curSpeed * Time.deltaTime);
                curDist = Quaternion.Angle(BKTransformManager.KuriRot, goal);
                yield return null;
            }
            BKTransformManager.KuriRot = goal;
            ResetCurMovementAction();
            StartNextMovement();
        }
        
        private void ResetCurMovementAction() {
            CurMovementAction = "";
        }
        #endregion
    }
}
