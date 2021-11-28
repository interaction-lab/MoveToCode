using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using UnityEngine;

namespace MoveToCode {
    public class ManipulationLogger : ReceiverBaseMonoBehavior {

        ManipulationHandler manipHandler;
        PressableButtonHoloLens2 pressButton;
        Interactable interactable;
        LoggingManager loggingManager;
        bool currentlyManipulating = false;

        void Start() {
            manipHandler = GetComponent<ManipulationHandler>();
            loggingManager = LoggingManager.instance;
            if (manipHandler != null) {
                manipHandler.OnManipulationStarted.AddListener(LogManipulationStart);
                manipHandler.OnManipulationEnded.AddListener(StopLogging);
            }
            pressButton = GetComponent<PressableButtonHoloLens2>();
            if (pressButton != null) {
                pressButton.ButtonPressed.AddListener(LogManipulationStartButtonPress);
                pressButton.ButtonReleased.AddListener(StopLogging);
            }
            interactable = GetComponent<Interactable>();
            if (interactable != null) {
                //interactable.OnClick.AddListener(StopLogging); // On release
            }
        }

        State lastState = null; // will automatically update on first state change
        public override void OnStateChange(InteractableStates state, Interactable source) {
            base.OnStateChange(state, source);
            if(lastState != state.CurrentState()){ // state change
                lastState = state.CurrentState();
                if(lastState.Name == "Pressed" && !currentlyManipulating){
                    LogManipulationStartButtonPress();
                }
                else{
                    StopLogging();
                }
            }
        }
        
        private void LogManipulationStartButtonPress() {
            currentlyManipulating = true;
            StartCoroutine(LogManipulationUntilDone());
        }

        private void LogManipulationStart(ManipulationEventData arg0) {
            LogManipulationStartButtonPress();
        }

        IEnumerator LogManipulationUntilDone() {
            while (currentlyManipulating) {
                loggingManager.UpdateLogColumn(ManipulationLoggingManager.GetColName(), gameObject.TryGetNiceNameOfObjectForLogging());
                yield return null;
            }
        }

        void StopLogging() {
            currentlyManipulating = false;
        }

        void StopLogging(ManipulationEventData arg0) {
            StopLogging();
        }
    }
}
