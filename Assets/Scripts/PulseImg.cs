using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoveToCode {
    public class PulseImg : MonoBehaviour {
        #region members
        Image img;
        public Image Img {
            get {
                if (img == null) {
                    img = GetComponent<Image>();
                    origButtonColor = img.color;
                    origMaterialColor = img.material.color;
                    materialWFullAlpha = img.material.color;
                    materialWFullAlpha.a = 1;
                }
                return img;
            }
        }

        Material _mat;
        Material MyMaterial {
            get {
                if (_mat == null) {
                    _mat = new Material(Img.material);
                    Img.material = _mat;
                }
                return _mat;
            }
        }

        Color origButtonColor, origMaterialColor, materialWFullAlpha;

        public bool IsPulsing = false;
        #endregion

        #region unity
        private void OnDisable() {
            MyMaterial.color = origMaterialColor;
            Img.color = origButtonColor;
        }
        #endregion

        #region public
        public void StartPulse(Color pulseColor) {
            StartCoroutine(PulseRoutine(pulseColor));
        }

        public void StopPulse() {
            IsPulsing = false;
        }
        #endregion

        #region private
        IEnumerator PulseRoutine(Color pulseColor) {
            if (IsPulsing) {
                yield break;
            }
            IsPulsing = true;
            while (IsPulsing) {
                Img.color = Color.Lerp(origButtonColor, pulseColor, Mathf.PingPong(Time.time, 1));
                MyMaterial.color = Color.Lerp(origMaterialColor, materialWFullAlpha, Mathf.PingPong(Time.time, 1));
                yield return null;
            }
            Img.color = origButtonColor;
            MyMaterial.color = origMaterialColor;
        }
        #endregion
    }
}
