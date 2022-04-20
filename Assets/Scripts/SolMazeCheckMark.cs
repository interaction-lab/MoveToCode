using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoveToCode {
    public class SolMazeCheckMark : Singleton<SolMazeCheckMark> {
        #region members
        Image _img;
        Image IMG {
            get {
                if (_img == null) {
                    _img = GetComponent<Image>();
                }
                return _img;
            }
        }
        #endregion

        #region unity
        #endregion

        #region public
        public void ToggleCheckMark(bool toggle) {
            IMG.enabled = toggle;
            if (toggle) {
                AudioManager.instance.PlayButtonClick();
            }
        }
        #endregion

        #region private
        #endregion
    }
}
