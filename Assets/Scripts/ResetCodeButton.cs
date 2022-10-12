using Microsoft.MixedReality.Toolkit.UI;
using System;
using TMPro;
using UnityEngine;

namespace MoveToCode {
    public class ResetCodeButton : MonoBehaviour {
        #region members
        ButtonConfigHelper bch;
        ButtonConfigHelper ButtonConfig {
            get {
                if (bch == null) {
                    bch = GetComponent<ButtonConfigHelper>();
                }
                return bch;
            }
        }
        Interpreter _interpreter;
        Interpreter InterpreterInstance {
            get {
                if (_interpreter == null) {
                    _interpreter = Interpreter.instance;
                }
                return _interpreter;
            }
        }
        PulseMeshRend _pulse = null;
        PulseMeshRend Pulse3DMeshRend {
            get {
                if (_pulse == null) {
                    _pulse = transform.GetChild(3).GetComponentInChildren<PulseMeshRend>(); // Flimsy, for in scene/3D UI
                }
                return _pulse;
            }
        }
        bool IsUIButton {
            get {
                return PulseIMG != null;
            }
        }

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
        SwitchModeButton smb;
        SwitchModeButton SwitchModeButton {
            get {
                if (smb == null) {
                    smb = SwitchModeButton.instance;
                }
                return smb;
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
            InterpreterInstance.OnCodeStart.AddListener(OnCodeStart);
            InterpreterInstance.OnCodeEnd.AddListener(OnCodeEnd);
            InterpreterInstance.OnCodeReset.AddListener(OnCodeReset);
            SwitchModeButton.OnSwitchToCodingMode.AddListener(OnSwitchToCodingMode);
            SwitchModeButton.OnSwitchToMazeBuildingMode.AddListener(OnSwitchToBuildingMode);
        }

        private void OnSwitchToBuildingMode() {
            transform.parent.gameObject.SetActive(false);
        }

        private void OnSwitchToCodingMode() {
            transform.parent.gameObject.SetActive(true);
        }
        #endregion

        #region public
        #endregion

        #region private
        private void OnCodeStart() {
            transform.parent.gameObject.SetActive(false);
        }

        private void OnCodeReset() {
            if (IsUIButton) {
                PulseIMG.StopPulse();
                TXTUI.text = "Reset";
            }
            else {
                Pulse3DMeshRend.StopPulse();
                ButtonConfig.MainLabelText = "Reset";
            }
        }

        private void OnCodeEnd() {
            transform.parent.gameObject.SetActive(true);
            if (IsUIButton) {
                // if the code is incorrect, pulse the button
                if (!MazeManagerInstance.ExerciseInFullyCompleteState) {
                    PulseIMG.StartPulse(Color.red);
                }
            }
            else {
                if (!MazeManagerInstance.ExerciseInFullyCompleteState) {
                    Pulse3DMeshRend.StartPulse(Color.red);
                }
            }
        }

        public void SetToNextMazeWPulse() {
            if (IsUIButton) {
                TXTUI.text = "Next Maze";
                PulseIMG.StopPulse();
                PulseIMG.StartPulse(Color.blue);
            }
            else {
                ButtonConfig.MainLabelText = "Next Maze";
                Pulse3DMeshRend.StopPulse();
                Pulse3DMeshRend.StartPulse(Color.blue);
            }
        }
        #endregion
    }
}
