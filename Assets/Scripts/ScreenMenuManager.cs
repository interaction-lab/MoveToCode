using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ScreenMenuManager : Singleton<ScreenMenuManager> {
        #region members
        ScreenToggleMenuButton _screenToggleMenuButton;
        public ScreenToggleMenuButton MyScreenToggleMenuButton {
            get {
                if (_screenToggleMenuButton == null) {
                    _screenToggleMenuButton = GetComponentInChildren<ScreenToggleMenuButton>();
                }
                return _screenToggleMenuButton;
            }
        }
        #endregion

        #region unity
        void OnEnable() {
            if (!MyScreenToggleMenuButton.ChildButtonsOn) {
                MyScreenToggleMenuButton.Toggle();
            }
        }
        #endregion

        #region public
        #endregion

        #region private
        #endregion
    }
}
