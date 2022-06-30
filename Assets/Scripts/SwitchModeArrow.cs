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

        void Awake() {
            MazeManager.instance.OnMazeLocked.AddListener(OnMazeLocked);
            SolMazeCheckMark.instance.OnMazeCorrect.AddListener(OnMazeCorrect);
            SolMazeCheckMark.instance.OnMazeIncorrect.AddListener(OnMazeIncorrect);
        }

        void OnMazeLocked() {
            TurnOff();
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
        }

        void TurnOff() {
            PulseIMG.StopPulse();
            gameObject.SetActive(false);
        }
    }
}
