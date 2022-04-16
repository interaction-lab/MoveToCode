using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MoveToCode {
    public class ARTrackingManager : Singleton<ARTrackingManager> {
        #region members
        public UnityEvent OnTrackingStarted, OnTrackingEnded;
        ARObjectManager arObjectManager;
        public ARObjectManager ARObjectManagerInstance {
            get {
                if (arObjectManager == null) {
                    arObjectManager = GetComponent<ARObjectManager>();
                }
                return arObjectManager;
            }
        }
        Interpreter interpreter;
        public Interpreter InterpreterInstance {
            get {
                if (interpreter == null) {
                    interpreter = Interpreter.instance;
                }
                return interpreter;
            }
        }

        public bool IsTracking { get; private set; } = false; // default to not tracking
        MazeManager mazeManager;
        MazeManager MazeManagerInstance {
            get {
                if (mazeManager == null) {
                    mazeManager = MazeManager.instance;
                }
                return mazeManager;
            }
        }
        #endregion

        #region unity
        void OnEnable() {
            StartTracking();
            InterpreterInstance.OnCodeReset.AddListener(OnCodeReset);
            InterpreterInstance.OnCodeStart.AddListener(OnCodeStart);
            MazeManagerInstance.OnMazeLocked.AddListener(OnMazeLocked);
            MazeManagerInstance.OnMazeUnlocked.AddListener(OnMazeUnlocked);
        }
        void OnDisable() {
            StopTracking();
            InterpreterInstance.OnCodeReset.RemoveListener(OnCodeReset);
            InterpreterInstance.OnCodeStart.RemoveListener(OnCodeStart);
            MazeManagerInstance.OnMazeLocked.RemoveListener(OnMazeLocked);
            MazeManagerInstance.OnMazeUnlocked.RemoveListener(OnMazeUnlocked);
        }
        #endregion

        #region public
        public void StartTracking() {
            if (!IsTracking) {
                IsTracking = true;
                OnTrackingStarted.Invoke();
            }
        }
        public void StopTracking() {
            if (IsTracking) {
                IsTracking = false;
                OnTrackingEnded.Invoke();
            }
        }

        #endregion

        #region private
        private void OnCodeReset() {
            StartTracking();
        }
        private void OnCodeStart() {
            StopTracking();
        }

        private void OnMazeUnlocked() {
            StartTracking();
        }

        private void OnMazeLocked() {
            StopTracking();
        }

        #endregion
    }
}