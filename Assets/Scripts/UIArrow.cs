using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace MoveToCode {
    public class UIArrow : MonoBehaviour {
        #region members
        public static string OuterArrowName = "UIArrowOuter", InnerArrowName = "UIArrowInner";
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

        ArrowPointPrefab _arrowPointPrefab;
        ArrowPointPrefab arrowPointPrefab {
            get {
                if (_arrowPointPrefab == null) {
                    _arrowPointPrefab = GetComponentInParent<ArrowPointPrefab>();
                }
                return _arrowPointPrefab;
            }
        }
        #endregion

        #region unity
        private void Awake() {
            Assert.IsTrue(transform.name == OuterArrowName || transform.name == InnerArrowName, "UIArrow must be named either " + OuterArrowName + " or " + InnerArrowName + "due to string comparisons in `ArrowPointPrefab.cs`");
        }
        void Update() {
            if (arrowPointPrefab.IsOffScreen && !arrowPointPrefab.IsBehindPlayer) {
                ArrowImage.enabled = true;
                UpdateArrowPosition();
            }
            else {
                ArrowImage.enabled = false;
            }
        }
        #endregion

        #region public
        public void SetColor(Color c) {
            ArrowImage.color = c;
        }
        #endregion

        #region private
        void UpdateArrowPosition() {
            Vector2 screenPos = new Vector2(
               (arrowPointPrefab.viewPortPos.x * Screen.width) - (Screen.width / 2f),
               (arrowPointPrefab.viewPortPos.y * Screen.height) - (Screen.height / 2f)
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
