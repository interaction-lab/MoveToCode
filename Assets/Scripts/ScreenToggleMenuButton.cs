using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoveToCode {
    public class ScreenToggleMenuButton : MonoBehaviour {
        #region members
        Button _button;
        Button MyButton {
            get {
                if (_button == null) {
                    _button = GetComponent<Button>();
                }
                return _button;
            }
        }

        GameObject _childButtons;
        GameObject ChildButtons {
            get {
                if (_childButtons == null) {
                    _childButtons = transform.GetChild(0).gameObject; // Flimsy, issue when moving objects around
                }
                return _childButtons;
            }
        }

        public bool ChildButtonsOn {
            get {
                return ChildButtons.activeSelf;
            }
        }
        #endregion

        #region unity
        private void Awake() {
            MyButton.onClick.AddListener(OnClick);
        }
        #endregion

        #region public
        public void Toggle() {
            ChildButtons.SetActive(!ChildButtons.activeSelf);
        }
        #endregion

        #region private
        private void OnClick() {
            Toggle();
        }
        #endregion
    }
}
