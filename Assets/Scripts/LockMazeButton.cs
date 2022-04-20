using Microsoft.MixedReality.Toolkit.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoveToCode {
    public class LockMazeButton : MonoBehaviour {
        #region members
        MazeManager _mazeManager;
        MazeManager MazeManagerInstance {
            get {
                if (_mazeManager == null) {
                    _mazeManager = MazeManager.instance;
                }
                return _mazeManager;
            }
        }
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

        Button _button;
        Button MyButton {
            get {
                if (_button == null) {
                    _button = GetComponent<Button>();
                }
                return _button;
            }
        }



        public bool IsScreenButton {
            get {
                return MyButton != null;
            }
        }

        TextMeshProUGUI _tmp;
        TextMeshProUGUI MyText {
            get {
                if (_tmp == null) {
                    _tmp = GetComponentInChildren<TextMeshProUGUI>();
                }
                return _tmp;
            }
        }
        #endregion

        #region unity
        private void Awake() {
            if (IsScreenButton) {
                MyButton.onClick.AddListener(OnScreenClick);
            }
            MazeManagerInstance.OnMazeLocked.AddListener(OnMazeLocked);
            MazeManagerInstance.OnMazeUnlocked.AddListener(OnMazeUnlocked);
            InterpreterInstance.OnCodeStart.AddListener(OnCodeStart);
            InterpreterInstance.OnCodeReset.AddListener(OnCodeReset);
        }
        #endregion

        #region public
        #endregion

        #region private
        private void OnCodeReset() {
            if (IsScreenButton) {
                transform.parent.gameObject.SetActive(true);
            }
            else {
                gameObject.SetActive(true);
            }
        }

        private void OnCodeStart() {
            if (IsScreenButton) {
                transform.parent.gameObject.SetActive(false);
            }
            else {
                gameObject.SetActive(false);
            }
        }

        private void OnMazeUnlocked() {
            if (IsScreenButton) {
                MyText.text = "Lock Maze";
            }
            else {
                ButtonConfig.MainLabelText = "Lock Maze";
            }
        }

        private void OnMazeLocked() {
            if (IsScreenButton) {
                MyText.text = "Unlock Maze";
            }
            else {
                ButtonConfig.MainLabelText = "Unlock Maze";
            }
        }

        private void OnScreenClick() {
            MazeManagerInstance.ToggleMazeLock();
        }
    }
    #endregion
}

