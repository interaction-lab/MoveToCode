using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using UnityEngine;

namespace MoveToCode {
    public class ManipulationLoggingManager : Singleton<ManipulationLoggingManager> {
        static string manipColName = "ManipulatingObject";

        private void Start() {
            LoggingManager.instance.AddLogColumn(manipColName, "");
            StartCoroutine(AddLoggersToManipulationHandlers());
        }

        IEnumerator AddLoggersToManipulationHandlers() {
            yield return null; // need to wait a frame before finding / adding components
            AddManipHandlerToComponent<ManipulationHandler>();
            AddManipHandlerToComponent<PressableButtonHoloLens2>();
            AddManipHandlerToComponent<Interactable>();
            AddManipHandlerToComponent<Button>();
        }

        private void AddManipHandlerToComponent<T>() {
            foreach (var go in Resources.FindObjectsOfTypeAll(typeof(T)) as Object[]) {
                GameObject g = (go as Component).gameObject;
                if (g.GetComponent<ManipulationLogger>() == null) {
                    g.AddComponent<ManipulationLogger>();
                }
            }
        }

        public static string GetColName() {
            return manipColName;
        }

    }
}
