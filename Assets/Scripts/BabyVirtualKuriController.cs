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
        public float speed = 1f;
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

        private Regex moveRe = new Regex(@"^" + MoveLogString);
        public bool IsMoving {
            get {
                return moveRe.IsMatch(CurMovementAction);
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

        Queue<float> MoveQueue { get; set; } = new Queue<float>();

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
            MoveQueue.Enqueue(dist);
            StartNextMovement();
        }

        public void TurnOverTime(float v) {
            Debug.Log("Turn " + v.ToString());
        }

        public void ResetOrigPos() {
            KuriPos = OriginalPosition;
        }

        #endregion

        #region private


        private void StartNextMovement() {
            if (!MoveQueue.Empty() && !IsMoving) {
                float dist = MoveQueue.Dequeue();
                StartCoroutine(GoToPosition(KuriPos + KuriForward * dist, dist > 0));
            }
        }


        private float goalDistDelta = 0.02f;
        public IEnumerator GoToPosition(Vector3 goal, bool forward) {
            if (IsMoving) {
                throw new InvalidOperationException("Baby Kuri already moving, check moveQueue queueing code");
            }
            CurMovementAction = MoveLogString + (forward ? "FORWARD" : "BACKWARD") + " to " + goal.ToString();
            float totalDist = Vector3.Distance(KuriPos, goal);
            float curDist = Vector3.Distance(KuriPos, goal);
            while (curDist > goalDistDelta) {
                Vector3 dir = goal - KuriPos;
                float curSpeed = speed * speedCurve.Evaluate(curDist / totalDist);
                KuriPos = KuriPos + dir * curSpeed * Time.deltaTime;
                curDist = Vector3.Distance(KuriPos, goal);
                yield return null;
            }
            KuriPos = goal;
            ResetCurMovementAction();
            StartNextMovement();
        }


        private void ResetCurMovementAction() {
            CurMovementAction = "";
        }
        #endregion
    }
}
