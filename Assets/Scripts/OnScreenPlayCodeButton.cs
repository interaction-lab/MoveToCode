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
            Interpreter.instance.OnCodeStart.AddListener(OnCodeStart);
            Interpreter.instance.OnCodeEnd.AddListener(OnCodeEnd);
        }
        #endregion

        #region private
        private void OnSwitchToBuildingMode() {
            transform.parent.gameObject.SetActive(false);
        }

        private void OnSwitchToCodingMode() {
            transform.parent.gameObject.SetActive(true);
        }

        private void OnCodeStart() {
            transform.parent.gameObject.SetActive(false);
        }

        private void OnCodeEnd() {
            transform.parent.gameObject.SetActive(true);
        }
        #endregion
    }
}
