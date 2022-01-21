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
        BabyKuriManager bkm;
        BabyKuriManager babyKuriManager {
            get {
                if (bkm == null) {
                    bkm = BabyKuriManager.instance;
                }
                return bkm;
            }
        }
        public Vector3 OriginalPosition { get; set; }

        public static string MoveLogString { get; } = "Moving ";
        public static string TurnLogString { get; } = "Turning ";

        private Regex moveRe = new Regex(@"^" + MoveLogString);
        private Regex turnRe = new Regex(@"^" + TurnLogString);
        public bool IsMoving {
            get {
                return moveRe.IsMatch(CurMovementAction) || turnRe.IsMatch(CurMovementAction);
            }
        }

        private Transform KuriTransform {
            get {
                return babyKuriManager.transform;
            }
        }
        Vector3 KuriForward {
            get {
                return KuriTransform.forward * -1f;
            }
        }
        private Vector3 KuriPos {
            get {
                return babyKuriManager.transform.position;
            }
            set {
                babyKuriManager.transform.position = value;
            }
        }
        private Quaternion KuriQuat {
            get {
                return KuriTransform.rotation;
            }
            set {
                KuriTransform.rotation = value;
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
            OriginalPosition = babyKuriManager.transform.position;
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

        public void ResetOrigPos() {
            KuriPos = OriginalPosition;
        }

        #endregion

        #region private
        private void StartNextMovement() {
            if (!MoveQueue.Empty() && !IsMoving) {
                KeyValuePair<Type, float> p = MoveQueue.Dequeue();
                if (p.Key == typeof(CodeBlockEnums.Move)) {
                    StartCoroutine(GoToPosition(KuriPos + KuriForward * p.Value, p.Value > 0));
                }
                else if (p.Key == typeof(CodeBlockEnums.Turn)) {
                    StartCoroutine(TurnToAngle(Quaternion.Euler(KuriQuat.eulerAngles + KuriTransform.up * p.Value), p.Value > 0));
                }
            }
        }


        private float goalDistDelta = 0.02f;
        public IEnumerator GoToPosition(Vector3 goal, bool forward) {
            if (IsMoving) {
                throw new InvalidOperationException("Baby Kuri already moving, check moveQueue queueing code");
            }
            CurMovementAction = MoveLogString + (forward ? "FORWARD" : "BACKWARD") + " to " + goal.ToString();
            float totalDist = Vector3.Distance(KuriPos, goal);
            float curDist = totalDist;
            while (curDist > goalDistDelta) {
                Vector3 dir = goal - KuriPos;
                float curSpeed = moveSpeed * speedCurve.Evaluate(curDist / totalDist);
                KuriPos = KuriPos + dir * curSpeed * Time.deltaTime;
                curDist = Vector3.Distance(KuriPos, goal);
                yield return null;
            }
            KuriPos = goal;
            ResetCurMovementAction();
            StartNextMovement();
        }

        private float goalDegreeDelta = 3f;
        public IEnumerator TurnToAngle(Quaternion goal, bool right) {
            if (IsMoving) {
                throw new InvalidOperationException("Baby Kuri already moving, check moveQueue queueing code");
            }
            CurMovementAction = MoveLogString + (right ? "RIGHT" : "LEFT") + " to " + goal.ToString();
            float totalDist = Quaternion.Angle(KuriQuat, goal);
            float curDist = totalDist;
            while (Mathf.Abs(curDist) > goalDegreeDelta) {
                float curSpeed = turnSpeed * speedCurve.Evaluate(curDist / totalDist);
                KuriQuat = Quaternion.Euler(KuriQuat.eulerAngles + KuriTransform.up * curSpeed * Time.deltaTime);
                curDist = Quaternion.Angle(KuriQuat, goal);
                yield return null;
            }
            KuriQuat = goal;
            ResetCurMovementAction();
            StartNextMovement();
        }



        private void ResetCurMovementAction() {
            CurMovementAction = "";
        }
        #endregion
    }
}
