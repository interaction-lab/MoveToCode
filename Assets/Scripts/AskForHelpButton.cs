using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoveToCode {
    public class AskForHelpButton : MonoBehaviour {
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
        Button b;
        Button MyButton {
            get {
                if (b == null) {
                    b = GetComponent<Button>();
                }
                return b;
            }
        }
        #endregion

        #region unity
        private void Awake() {
            InterpreterInstance.OnCodeEnd.AddListener(OnCodeEnd);
            InterpreterInstance.OnCodeStart.AddListener(OnCodeStart);
            MyButton.onClick.AddListener(OnButtonClick);
        }
        #endregion

        #region public
        #endregion

        #region private
        private void OnCodeEnd() {
            MyButton.enabled = true;
        }

        private void OnCodeStart() {
            MyButton.enabled = false;
        }

        void OnButtonClick() {
            TutorKuriManager.instance.AskForHelp();
        }
        #endregion
    }
}
