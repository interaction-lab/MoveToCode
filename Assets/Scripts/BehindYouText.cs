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

        ArrowPointPrefab _arrowPoint;
        ArrowPointPrefab arrowPoint {
            get {
                if (_arrowPoint == null) {
                    _arrowPoint = GetComponentInParent<ArrowPointPrefab>();
                }
                return _arrowPoint;
            }
        }
        #endregion
        #region unity
        void Update() {
            if (arrowPoint.IsBehindPlayer && arrowPoint.IsOffScreen) {
                textMeshProUGUI.enabled = true;
            }
            else {
                textMeshProUGUI.enabled = false;
            }
        }
        #endregion
        #region public
        public void SetText(string s) {
            textMeshProUGUI.text = s;
        }
        #endregion
    }
}
