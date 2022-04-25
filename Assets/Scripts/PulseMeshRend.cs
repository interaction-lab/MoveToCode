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

        Material _mat;
        Material MyMaterial {
            get {
                if (_mat == null) {
                    _mat = new Material(MyRend.material);
                    MyRend.material = _mat;
                }
                return _mat;
            }
        }

        public bool IsPulsing = false;
        #endregion

        #region unity
        private void OnDisable() {
            MyMaterial.color = origColor;
        }
        #endregion

        #region public
        public void StartPulse(Color _c) {
            StartCoroutine(PulseRoutine(_c));
        }

        public void StopPulse() {
            IsPulsing = false;
        }
        #endregion

        #region private
        IEnumerator PulseRoutine(Color _c) {
            if (IsPulsing) {
                yield break;
            }
            IsPulsing = true;
            while (IsPulsing) {
                MyMaterial.color = Color.Lerp(origColor, _c, Mathf.PingPong(Time.time, 1));
                yield return null;
            }
            MyMaterial.color = origColor;
        }
        #endregion
    }
}
