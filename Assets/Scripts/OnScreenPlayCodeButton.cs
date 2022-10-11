using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class OnScreenPlayCodeButton : MonoBehaviour {
        #region members
        #endregion

        #region unity
        private void Awake() {
            SwitchModeButton.instance.OnSwitchToCodingMode.AddListener(OnSwitchToCodingMode);
            SwitchModeButton.instance.OnSwitchToMazeBuildingMode.AddListener(OnSwitchToBuildingMode);
        }
        #endregion

        #region private
        private void OnSwitchToBuildingMode() {
            transform.parent.gameObject.SetActive(false);
        }

        private void OnSwitchToCodingMode() {
            transform.parent.gameObject.SetActive(true);
        }
        #endregion
    }
}
