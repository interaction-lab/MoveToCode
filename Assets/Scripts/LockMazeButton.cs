using Microsoft.MixedReality.Toolkit.UI;
using System;
using UnityEngine;

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
        #endregion

        #region unity
        private void Awake() {
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
            gameObject.SetActive(true);
        }

        private void OnCodeStart() {
            gameObject.SetActive(false);
        }

        private void OnMazeUnlocked() {
            ButtonConfig.MainLabelText = "Lock Maze";
        }

        private void OnMazeLocked() {
            ButtonConfig.MainLabelText = "Unlock Maze";
        }
    }
    #endregion
}

