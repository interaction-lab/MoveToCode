using System.Collections;
using UnityEngine;
using System.Text.RegularExpressions;

namespace MoveToCode {
    public class BabyVirtualKuriController : MonoBehaviour {

        #region members
        public string CurMovementAction = "";
        public static string babyKuriMovementActionCol = "babyKuriMovementAction";
        public float speed = 0.001f;
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

        public static string MoveLogString { get; } = "Moving To ";
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
            CurMovementAction = "Move";
            CurMovementAction += dist < 0 ? " Backward" : " Forward";
            CurMovementAction += dist.ToString();
            StartCoroutine(GoToPosition(KuriPos + KuriForward * dist));
        }

        public void ResetOrigPos() {
            KuriPos = OriginalPosition;
        }

        #endregion

        #region private
        private float goalDistDelta = 0.05f;
        public IEnumerator GoToPosition(Vector3 goal) {
            if (IsMoving) {
                Debug.LogError("Already moving");
            }
            CurMovementAction = MoveLogString + goal.ToString();
            while (Vector3.Distance(KuriPos, goal) > goalDistDelta) {
                Vector3 dir = goal - KuriPos;
                KuriPos = KuriPos + dir * speed;
                yield return null;
            }
            ResetCurMovementAction();
        }


        private void ResetCurMovementAction() {
            CurMovementAction = "";
        }
        #endregion
    }
}
