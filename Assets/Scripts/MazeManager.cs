using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MazeManager : Singleton<MazeManager> {
        #region members
        #endregion

        #region unity
        private void OnEnable() {
#if UNITY_EDITOR
            AddManipulationHandlersForUnityEditor();
#endif
        }
        #endregion

        #region public
        public GameObject GetMazeObject(string name) {
            foreach (Transform child in transform) {
                if (child.name == name) {
                    return child.gameObject;
                }
            }
            return null;
        }
        #endregion

        #region private
        private void AddManipulationHandlersForUnityEditor() {
            foreach (GameObject child in transform) {
                ManipulationHandler manipHandler = child.AddComponent<ManipulationHandler>();
                manipHandler.ManipulationType = ManipulationHandler.HandMovementType.OneHandedOnly;
            }
        }
        #endregion
    }
}
