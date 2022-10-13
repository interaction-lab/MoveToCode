using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MoveToCode {
    public class OnScreenPlayCodeButton : MonoBehaviour {
        #region members
        PulseImg _pulseImg;
        PulseImg PulseIMG {
            get {
                if (_pulseImg == null) {
                    _pulseImg = transform.parent.GetComponent<PulseImg>(); // FLimsy, for 2D UI
                }
                return _pulseImg;
            }
        }
        TextMeshProUGUI tmpUI;
        TextMeshProUGUI TXTUI {
            get {
                if (tmpUI == null) {
                    tmpUI = GetComponentInChildren<TextMeshProUGUI>();
                }
                return tmpUI;
            }
        }
        MazeManager mm;
        MazeManager MazeManagerInstance {
            get {
                if (mm == null) {
                    mm = MazeManager.instance;
                }
                return mm;
            }
        }
        #endregion

        #region unity
        private void Awake() {
            SwitchModeButton.instance.OnSwitchToCodingMode.AddListener(OnSwitchToCodingMode);
            SwitchModeButton.instance.OnSwitchToMazeBuildingMode.AddListener(OnSwitchToBuildingMode);
            Interpreter.instance.OnCodeStart.AddListener(OnCodeStart);
            Interpreter.instance.OnCodeEnd.AddListener(OnCodeEnd);
            Interpreter.instance.OnCodeReset.AddListener(OnCodeReset);
        }
        #endregion
        #region public
        public void SetToNextMazeWPulse() {
            TXTUI.text = "Next Maze";
            PulseIMG.StopPulse();
            PulseIMG.StartPulse(Color.blue);
        }
        #endregion

        #region private
        private void OnSwitchToBuildingMode() {
            transform.parent.gameObject.SetActive(false);
        }

        private void OnSwitchToCodingMode() {
            transform.parent.gameObject.SetActive(true);
        }

        private void OnCodeStart() {
            transform.parent.gameObject.SetActive(false);
        }

        private void OnCodeEnd() {
            transform.parent.gameObject.SetActive(true);
            // if the code is incorrect, pulse the button
            if (!MazeManagerInstance.ExerciseInFullyCompleteState) {
                PulseIMG.StartPulse(Color.red);
                TXTUI.text = "Reset Code";
            }
        }

        private void OnCodeReset() {
            PulseIMG.StopPulse();
            TXTUI.text = "Play Code";
        }
        #endregion
    }
}
