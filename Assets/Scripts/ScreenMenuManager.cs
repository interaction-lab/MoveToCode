using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ScreenMenuManager : Singleton<ScreenMenuManager> {
        #region members
        public ResetCodeButton MyResetCodeButton {
            get {
                return transform.GetChild(1).GetComponentInChildren<ResetCodeButton>(); // TODO: hacky and flimsy but whatever at this point
            }
        }
        #endregion

        #region unity
        #endregion

        #region public
        #endregion

        #region private
        #endregion
    }
}
