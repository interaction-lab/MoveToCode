using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MoveToCode {
    public class UIArrow : MonoBehaviour {
        #region members
        Image _arrowImage;
        Image ArrowImage {
            get {
                if (_arrowImage == null) {
                    _arrowImage = GetComponent<Image>();
                }
                return _arrowImage;
            }
        }

        RectTransform _rectTransform;
        RectTransform rectTransform {
            get {
                if (_rectTransform == null) {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }

        ViewPortManager _viewPortManager;
        ViewPortManager viewPortManager {
            get {
                if (_viewPortManager == null) {
                    _viewPortManager = ViewPortManager.instance; ;
                }
                return _viewPortManager;
            }
        }

        #endregion

        #region unity
        void Update() {
            if (viewPortManager.IsOffScreen && !viewPortManager.IsBehindPlayer) {
                ArrowImage.enabled = true;
                UpdateArrowPosition();
            }
            else {
                ArrowImage.enabled = false;
            }
        }
        #endregion

        #region public
        #endregion

        #region private
        void UpdateArrowPosition() {
            Vector2 screenPos = new Vector2(
               (viewPortManager.viewPortPos.x * Screen.width) - (Screen.width / 2f),
               (viewPortManager.viewPortPos.y * Screen.height) - (Screen.height / 2f)
           );
            // get largest offset from center
            float maxOffset = Mathf.Max(
                Mathf.Abs(screenPos.x),
                Mathf.Abs(screenPos.y)
            );
            // put into viewport space
            screenPos = (screenPos / (maxOffset * 2f)) + new Vector2(0.5f, 0.5f);

            // set arrow position to screenPos
            rectTransform.anchorMin = screenPos;
            rectTransform.anchorMax = screenPos;
            rectTransform.anchoredPosition = Vector2.zero;

            // convert screenPos to back to screen space
            screenPos = (screenPos - new Vector2(0.5f, 0.5f)) * maxOffset * 2f;
            // get direction from screenPos to center
            Vector2 direction = screenPos.normalized;
            // get angle from direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // set arrow angle to angle
            rectTransform.localEulerAngles = new Vector3(0, 0, angle + 180);
        }
        #endregion
    }
}
