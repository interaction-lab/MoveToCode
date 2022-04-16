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
                    _pulse = transform.GetChild(3).GetComponentInChildren<PulseMeshRend>(); // Flimsy
                }
                return _pulse;
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
            Pulse.StopPulse();
        }

        private void OnCodeEnd() {
            Pulse.StartPulse();
        }
        #endregion
    }
}
