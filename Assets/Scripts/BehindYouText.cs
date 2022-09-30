using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MoveToCode {
    public class BehindYouText : MonoBehaviour {
        #region members
        TextMeshProUGUI _textMeshProUGUI;
        TextMeshProUGUI textMeshProUGUI {
            get {
                if (_textMeshProUGUI == null) {
                    _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
                }
                return _textMeshProUGUI;
            }
        }

        ViewPortManager _viewPortManager;
        ViewPortManager viewPortManager {
            get {
                if (_viewPortManager == null) {
                    _viewPortManager = ViewPortManager.instance;
                }
                return _viewPortManager;
            }
        }
        #endregion

        #region unity
        void Update() {
            if (viewPortManager.IsBehindPlayer && viewPortManager.IsOffScreen) {
                textMeshProUGUI.enabled = true;
            }
            else {
                textMeshProUGUI.enabled = false;
            }
        }
        #endregion
    }
}
