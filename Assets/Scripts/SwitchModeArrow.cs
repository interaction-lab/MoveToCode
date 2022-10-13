using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SwitchModeArrow : MonoBehaviour {
        PulseImg _pulseImg;
        public PulseImg PulseIMG {
            get {
                if (_pulseImg == null) {
                    _pulseImg = GetComponent<PulseImg>();
                }
                return _pulseImg;
            }
        }
        Vector3 origPos;

        void Awake() {
            MazeManager.instance.OnMazeLocked.AddListener(OnMazeLocked);
            MazeManager.instance.OnMazeUnlocked.AddListener(OnMazeUnlocked);
            SolMazeCheckMark.instance.OnMazeCorrect.AddListener(OnMazeCorrect);
            SolMazeCheckMark.instance.OnMazeIncorrect.AddListener(OnMazeIncorrect);
            origPos = transform.position;
        }

        void OnMazeLocked() {
            TurnOff();
        }

        void OnMazeUnlocked() {
            if (MazeManager.instance.IsSameAsSolutionMaze()) {
                TurnOn();
            }
        }

        void OnMazeCorrect() {
            if (!MazeManager.instance.IsLocked) {
                TurnOn();
            }
        }

        void OnMazeIncorrect() {
            TurnOff();
        }

        void TurnOn() {
            gameObject.SetActive(true);
            PulseIMG.StartPulse(Color.white);
            StartCoroutine(MoveRightThenBack());
        }

        void TurnOff() {
            moving = false;
            PulseIMG.StopPulse();
            gameObject.SetActive(false);
        }

        bool moving = false;
        IEnumerator MoveRightThenBack() {
            if (moving) {
                yield break;
            }
            moving = true;
            RectTransform rt = GetComponent<RectTransform>();
            rt.localPosition = new Vector3(100, 0, 0); // origPos just hacked

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
