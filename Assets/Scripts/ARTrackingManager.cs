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

        public bool IsTracking{ get; private set; } = false; // default to not tracking
        
        #endregion

        #region unity
        void OnEnable(){
            StartTracking();
            InterpreterInstance.OnCodeReset.AddListener(OnCodeReset);
            InterpreterInstance.OnCodeStart.AddListener(OnCodeStart);
        }
        void OnDisable(){
            StopTracking();
            InterpreterInstance.OnCodeReset.RemoveListener(OnCodeReset);
            InterpreterInstance.OnCodeStart.RemoveListener(OnCodeStart);
        }
        #endregion

        #region public
        public void StartTracking() {
            if(!IsTracking){
                IsTracking = true;
                OnTrackingStarted.Invoke();
            }
        }
        public void StopTracking() {
            if(IsTracking){
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
        
        #endregion
    }
}