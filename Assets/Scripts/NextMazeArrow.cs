using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class NextMazeArrow : MonoBehaviour {
        PulseImg _pulseImg = null;
        public PulseImg PulseIMG {
            get {
                if (_pulseImg == null) {
                    _pulseImg = GetComponent<PulseImg>();
                }
                return _pulseImg;
            }
        }
        Vector3 origPos;

        private void Awake() {
            origPos = transform.position;
        }

        public void TurnOn() {
            gameObject.SetActive(true);
            PulseIMG.StartPulse(Color.white);
            // start a coroutine to move right some then back
            StartCoroutine(MoveRightThenBack());
        }

        public void TurnOff() {
            moving = false;
            PulseIMG.StopPulse();
            gameObject.SetActive(false);
        }

        bool moving = false;
        IEnumerator MoveRightThenBack() {
            if (moving) {
                yield break;
            }
            RectTransform rt = GetComponent<RectTransform>();
            rt.localPosition = new Vector3(100, 0, 0); // origPos just hacked
            moving = true;

            float time = 0;
            float duration = 0.5f;
            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + new Vector3(30f, 0, 0);
            while (enabled && moving) {
                while (time < duration) {
                    transform.position = Vector3.Lerp(startPos, endPos, time / duration);
                    time += Time.deltaTime;
                    yield return null;
                }
                while (time > 0) {
                    transform.position = Vector3.Lerp(startPos, endPos, time / duration);
                    time -= Time.deltaTime;
                    yield return null;
                }
            }


            moving = false;
        }
    }
}
