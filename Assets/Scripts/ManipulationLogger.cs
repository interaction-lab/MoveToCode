using Microsoft.MixedReality.Toolkit.UI;
using System;
using UnityEngine;

namespace MoveToCode {
    public class ManipulationLogger : MonoBehaviour {

        ManipulationHandler manipHandler;
        PressableButtonHoloLens2 pressButton;

        void Start() {
            manipHandler = GetComponent<ManipulationHandler>();
            if (manipHandler != null) {
                manipHandler.OnManipulationStarted.AddListener(LogManipulationStart);
            }
            pressButton = GetComponent<PressableButtonHoloLens2>();
            if (pressButton != null) {
                pressButton.ButtonPressed.AddListener(LogManipulationStartButtonPress);
            }
        }

        private void LogManipulationStartButtonPress() {
            LoggingManager.instance.UpdateLogColumn(ManipulationLoggingManager.GetColName(), gameObject.TryGetNiceNameOfObjectForLogging());
        }

        private void LogManipulationStart(ManipulationEventData arg0) {
            LoggingManager.instance.UpdateLogColumn(ManipulationLoggingManager.GetColName(), gameObject.TryGetNiceNameOfObjectForLogging());
        }
    }
}
