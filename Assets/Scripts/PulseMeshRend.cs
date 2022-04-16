using System.Collections;
using UnityEngine;

namespace MoveToCode {
    public class PulseMeshRend : MonoBehaviour {
        #region members
        MeshRenderer myRend;
        Color origColor;
        MeshRenderer MyRend {
            get {
                if (myRend == null) {
                    myRend = GetComponent<MeshRenderer>();
                    origColor = myRend.material.color;
                }
                return myRend;
            }
        }

        public bool IsPulsing = false;
        #endregion

        #region unity
        #endregion

        #region public
        public void StartPulse() {
            StartCoroutine(PulseRoutine());
        }

        public void StopPulse() {
            IsPulsing = false;
        }
        #endregion

        #region private
        IEnumerator PulseRoutine() {
            if (IsPulsing) {
                yield break;
            }
            IsPulsing = true;
            while (IsPulsing) {
                MyRend.material.color = Color.Lerp(origColor, Color.red, Mathf.PingPong(Time.time, 1));
                yield return null;
            }
            MyRend.material.color = origColor;
        }
        #endregion
    }
}
