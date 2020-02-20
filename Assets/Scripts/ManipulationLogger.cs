using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace MoveToCode {
    public class ManipulationLogger : MonoBehaviour {

        ManipulationHandler manipHandler;

        void Start() {
            manipHandler = GetComponent<ManipulationHandler>();
            manipHandler.OnManipulationStarted.AddListener(LogManipulationStart);
        }

        private void LogManipulationStart(ManipulationEventData arg0) {
            LoggingManager.instance.UpdateLogColumn(ManipulationLoggingManager.GetColName(), name);
        }
    }
}
