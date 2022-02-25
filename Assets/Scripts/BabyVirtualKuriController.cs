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
        public Vector3 OriginalPosition { get; private set; }
        public Quaternion OriginalRotation { get; private set; }
        public Color OriginalColor { get; private set; }

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

        private Quaternion KuriRot {
            get {
                return KuriTransform.rotation;
            }
            set {
                KuriTransform.rotation = value;
            }
        }

        public Color KuriColor {
            get {
                return bodyPlateRend.material.color;
            }
            private set {
                bodyPlateRend.material.color = value;
            }
        }

        bool origStateIsSet = false;

        /// <summary>
        /// Type is the type of movement
        /// float is the amount of movement (degrees for turns, dist in meters for moving)
        /// </summary>
        Queue<KeyValuePair<Type, float>> MoveQueue { get; set; } = new Queue<KeyValuePair<Type, float>>();

        #endregion

        #region unity
        private void Awake() {
            LoggingManager.instance.AddLogColumn(babyKuriMovementActionCol, "");
            if (!origStateIsSet) {
                SetOrigState();
            }
        }

        private void SetOrigState() {
            OriginalPosition = KuriPos;
            OriginalRotation = KuriRot;
            OriginalColor = KuriColor;
            origStateIsSet = true;
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
            if (!origStateIsSet) {
                SetOrigState();
            }
            KuriPos = OriginalPosition;
            KuriRot = OriginalRotation;
            KuriColor = OriginalColor;
        }

        public void SetColor(Color color) {
            KuriColor = color;
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
                    StartCoroutine(TurnToAngle(Quaternion.Euler(KuriRot.eulerAngles + KuriTransform.up * p.Value), p.Value > 0));
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
            float totalDist = Quaternion.Angle(KuriRot, goal);
            float curDist = totalDist;
            while (Mathf.Abs(curDist) > goalDegreeDelta) {
                float curSpeed = turnSpeed * speedCurve.Evaluate(curDist / totalDist);
                KuriRot = Quaternion.Euler(KuriRot.eulerAngles + KuriTransform.up * curSpeed * Time.deltaTime);
                curDist = Quaternion.Angle(KuriRot, goal);
                yield return null;
            }
            KuriRot = goal;
            ResetCurMovementAction();
            StartNextMovement();
        }



        private void ResetCurMovementAction() {
            CurMovementAction = "";
        }
        #endregion
    }
}
