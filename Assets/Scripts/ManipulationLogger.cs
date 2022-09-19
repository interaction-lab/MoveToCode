using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MoveToCode {
    public class ManipulationLogger : ReceiverBaseMonoBehavior {

        ManipulationHandler manipHandler;
        PressableButtonHoloLens2 pressButton;
        Interactable interactable;
        Button button;
        LoggingManager loggingManager;
        public static bool currentlyManipulating = false;
        public static string CurAction {get;set;} = "";
        string grabInteractableS = "GrabInteractable", pressButtonS = "PressButton";

        void Start() {
            manipHandler = GetComponent<ManipulationHandler>();
            loggingManager = LoggingManager.instance;
            if (manipHandler != null) {
                manipHandler.OnManipulationStarted.AddListener(LogManipulationStart);
                manipHandler.OnManipulationEnded.AddListener(StopLogging);
            }
            pressButton = GetComponent<PressableButtonHoloLens2>();
            if (pressButton != null) {
                pressButton.ButtonPressed.AddListener(LogManipulationStart);
                pressButton.ButtonReleased.AddListener(StopLogging);
            }
            interactable = GetComponent<Interactable>();
            if (interactable != null) {
                // handled by `OnStateChange`
            }
            button = GetComponent<Button>();
            if (button != null) {
                button.onClick.AddListener(LogManipulationStart);
                button.onClick.AddListener(StopLogging);
            }
        }

        State myLastState = null; // will automatically update on first state change
        public override void OnStateChange(InteractableStates state, Interactable source) {
            base.OnStateChange(state, source);
            if(myLastState != state.CurrentState()){ // state change
                myLastState = state.CurrentState();
                if(myLastState.Name == "Pressed" && !currentlyManipulating){
                    CurAction = grabInteractableS;
                    LogManipulationStart();
                }
                else{
                    StopLogging();
                }
            }
        }
        
        private void LogManipulationStart() {
            currentlyManipulating = true;
            if(CurAction == ""){
                CurAction = pressButtonS;
            }
            StartCoroutine(LogManipulationUntilDone());
        }

        private void LogManipulationStart(ManipulationEventData arg0) {
            CurAction = grabInteractableS;
            LogManipulationStart();
        }

        IEnumerator LogManipulationUntilDone() {
            while (currentlyManipulating) {
                loggingManager.UpdateLogColumn(ManipulationLoggingManager.GetColName(), gameObject.TryGetNiceNameOfObjectForLogging());
                yield return null;
            }
            CurAction = "";
        }

        void StopLogging() {
            currentlyManipulating = false;
        }

        void StopLogging(ManipulationEventData arg0) {
            StopLogging();
        }

        void OnDisable(){
            StopLogging();
        }
    }
}
