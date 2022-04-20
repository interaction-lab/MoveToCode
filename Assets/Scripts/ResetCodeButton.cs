using Microsoft.MixedReality.Toolkit.UI;
using System;
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
        PulseMeshRend _pulse;
        PulseMeshRend Pulse {
            get {
                if (_pulse == null) {
                    _pulse = transform.GetChild(3).GetComponentInChildren<PulseMeshRend>(); // Flimsy, for in scene/3D UI
                }
                return _pulse;
            }
        }
        bool IsUIButton {
            get {
                return PulseImg != null;
            }
        }

        PulseImg _pulseImg;
        PulseImg PulseImg {
            get {
                if (_pulseImg == null) {
                    _pulseImg = transform.parent.GetComponent<PulseImg>(); // FLimsy, for 2D UI
                }
                return _pulseImg;
            }
        }
        #endregion

        #region unity
        private void Awake() {
            InterpreterInstance.OnCodeEnd.AddListener(OnCodeEnd);
            InterpreterInstance.OnCodeReset.AddListener(OnCodeReset);
        }
        #endregion

        #region public
        #endregion

        #region private
        private void OnCodeReset() {
            if (IsUIButton) {
                PulseImg.StopPulse();
            }
            else {
                Pulse.StopPulse();
            }
        }

        private void OnCodeEnd() {
            if (IsUIButton) {
                PulseImg.StartPulse();
            }
            else {
                Pulse.StartPulse();
            }
        }
        #endregion
    }
}
