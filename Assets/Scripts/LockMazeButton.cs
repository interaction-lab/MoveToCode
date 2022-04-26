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

        public GameObject screenPlayButtonObject;
        public GameObject screenResetButtonObject;
        public TextMeshProUGUI modeText;
        PulseImg _pulseImg;
        PulseImg PulseIMG {
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
            if (IsScreenButton) {
                MyButton.onClick.AddListener(OnScreenClick);
            }
            MazeManagerInstance.OnMazeLocked.AddListener(OnMazeLocked);
            MazeManagerInstance.OnMazeUnlocked.AddListener(OnMazeUnlocked);
            InterpreterInstance.OnCodeStart.AddListener(OnCodeStart);
            InterpreterInstance.OnCodeReset.AddListener(OnCodeReset);
            ExerciseManager.instance.OnCyleNewExercise.AddListener(OnNewExercise);
            SolMazeCheckMark.instance.OnMazeCorrect.AddListener(OnMazeCorrect);
            SolMazeCheckMark.instance.OnMazeIncorrect.AddListener(OnMazeIncorrect);
            // need onmazecorrect and onmazeincorrect

            OnMazeUnlocked();
        }
        #endregion

        #region public
        #endregion

        #region private
        private void OnMazeCorrect(){
            PulseIMG.StartPulse(Color.green);
        }
        private void OnMazeIncorrect(){
            PulseIMG.StopPulse();
        }
        private void OnNewExercise() {
            if (MazeManagerInstance.IsLocked) {
                OnScreenClick(); // fake a click when the new exercise is loaded
            }
        }
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
            string newTxt = "Switch Mode";
            if (IsScreenButton) {
                MyText.text = newTxt;
                screenPlayButtonObject.SetActive(false);
                screenResetButtonObject.SetActive(false);
                modeText.text = "Mode 1: Maze Building";
            }
            else {
                ButtonConfig.MainLabelText = newTxt;
            }
            CodeBlockManager.instance.HideCodeBlocks();
        }

        private void OnMazeLocked() {
            string newTxt = "Switch Mode";
            if (IsScreenButton) {
                MyText.text = newTxt;
                screenPlayButtonObject.SetActive(true);
                screenResetButtonObject.SetActive(true);
                modeText.text = "Mode 2: Coding";
            }
            else {
                ButtonConfig.MainLabelText = newTxt;
            }
            CodeBlockManager.instance.ShowCodeBlocks();
            PulseIMG.StopPulse();
        }

        private void OnScreenClick() {
            MazeManagerInstance.ToggleMazeLock();
        }
    }
    #endregion
}

