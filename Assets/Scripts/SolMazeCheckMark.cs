using System;
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
        private void Awake() {
            ToggleCheckMark();
        }
        #endregion

        #region public
        public void ToggleCheckMark() {
            // wait for end of frame to check for solution using a coroutine
            StartCoroutine(ToggleCheckMarkCoroutine());
        }

        private IEnumerator ToggleCheckMarkCoroutine() {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame(); // hack for some race conditions
            bool toggle = MazeManager.instance.ContainsSolutionMaze();
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
