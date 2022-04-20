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
                    origColor = img.color;
                }
                return img;
            }
        }
        Color origColor;

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
                Img.color = Color.Lerp(origColor, Color.red, Mathf.PingPong(Time.time, 1));
                yield return null;
            }
            Img.color = origColor;
        }
        #endregion
    }
}
