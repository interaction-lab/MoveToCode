using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoveToCode {
    /// <summary>
    /// Main button in top left corner
    /// Also manages the states of the play and reset buttons(while probably shouldn't and that should be abstracted higher but whateva)
    /// </summary>
    public class SwitchModeButton : MonoBehaviour {
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
            MyButton.onClick.AddListener(OnScreenClick);
            MazeManagerInstance.OnMazeLocked.AddListener(OnMazeLocked);
            MazeManagerInstance.OnMazeUnlocked.AddListener(OnMazeUnlocked);
            InterpreterInstance.OnCodeStart.AddListener(OnCodeStart);
            InterpreterInstance.OnCodeReset.AddListener(OnCodeReset);
            ExerciseManager.instance.OnCyleNewExercise.AddListener(OnNewExercise);
            SolMazeCheckMark.instance.OnMazeCorrect.AddListener(OnMazeCorrect);
            SolMazeCheckMark.instance.OnMazeIncorrect.AddListener(OnMazeIncorrect);
            OnMazeUnlocked();
        }
        #endregion

        #region public
        #endregion

        #region private
        private void OnMazeCorrect() {
            PulseIMG.StartPulse(Color.green);
        }
        private void OnMazeIncorrect() {
            PulseIMG.StopPulse();
        }
        private void OnNewExercise() {
            PulseIMG.StopPulse();
            if (MazeManagerInstance.IsLocked) {
                OnScreenClick(); // fake a click when the new exercise is loaded, this will transition from locked -> onlocked
            }
        }
        private void OnCodeReset() {
            MyButton.enabled = true;
        }

        private void OnCodeStart() {
            MyButton.enabled = false;
        }

        private void OnMazeUnlocked() {
            screenPlayButtonObject.SetActive(false);
            screenResetButtonObject.SetActive(false);
            modeText.text = "Mode 1: Maze Building";
            CodeBlockManager.instance.HideCodeBlocks();
        }

        private void OnMazeLocked() {
            screenPlayButtonObject.SetActive(true);
            screenResetButtonObject.SetActive(true);
            modeText.text = "Mode 2: Coding";

            CodeBlockManager.instance.ShowCodeBlocks();
            PulseIMG.StopPulse();
        }

        private void OnScreenClick() {
            MazeManagerInstance.ToggleMazeLock(); // the state of everything should really be up here and not in MazeManager
        }
    }
    #endregion
}

