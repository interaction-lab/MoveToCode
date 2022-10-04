using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace MoveToCode {
    public class ArrowPointPrefab : MonoBehaviour {
        #region members
        private Transform targetTransform = null;
        private Vector3 offSet = Vector3.zero;
        public bool IsBehindPlayer, IsOffScreen;
        public Vector3 worldTarget, screenPos, targetRelToCam, cappedScreenPos, viewPortPos;
        public UnityEvent OnEnterViewPort, OnExitViewport;
        bool _isInViewPort;
        bool wasOutOfView = false;
        bool updatingWhenObjectIsInactive = false; // needed to avoid infinte recursion on inactive updating on IsInViewPort
        public bool IsInViewPort {
            get {
                if (!gameObject.activeSelf && !updatingWhenObjectIsInactive) {
                    updatingWhenObjectIsInactive = true;
                    UpdateAllTargets();
                    UpdateIsInViewPort(); // need to update just in case someone checks when it's disabled
                }
                else {
                    updatingWhenObjectIsInactive = false;
                }
                return _isInViewPort;
            }
        }
        Camera _mainCam;
        Camera MainCam {
            get {
                if (_mainCam == null) {
                    _mainCam = Camera.main;
                }
                return _mainCam;
            }
        }
        UIArrow _outerArrow, _innerArrow;
        UIArrow outerArrow {
            get {
                if (_outerArrow == null) {
                    _outerArrow = transform.Find(UIArrow.OuterArrowName).GetComponent<UIArrow>();
                }
                return _outerArrow;
            }
        }
        UIArrow innerArrow {
            get {
                if (_innerArrow == null) {
                    _innerArrow = transform.Find(UIArrow.InnerArrowName).GetComponent<UIArrow>();
                }
                return _innerArrow;
            }
        }

        BehindYouText _behindYouText;
        BehindYouText behindYouText {
            get {
                if (_behindYouText == null) {
                    _behindYouText = GetComponentInChildren<BehindYouText>();
                }
                return _behindYouText;
            }
        }
        #endregion

        #region unity
        void Update() {
            if (targetTransform == null) {
                return;
            }
            UpdateAllTargets();
            UpdateIsInViewPort();
        }
        #endregion

        #region public
        public void Set(Transform t, Vector3 os, Color outerArrowColor, Color innerArrowColor, string text) {
            targetTransform = t;
            offSet = os;
            outerArrow.SetColor(outerArrowColor);
            innerArrow.SetColor(innerArrowColor);
            behindYouText.SetText(text);
        }
        public void TurnOff() {
            gameObject.SetActive(false);
        }
        public void TurnOn() {
            gameObject.SetActive(true);
        }
        #endregion

        #region private
        void UpdateAllTargets() {
            worldTarget = targetTransform.position + offSet;
            screenPos = MainCam.WorldToScreenPoint(worldTarget);
            targetRelToCam = MainCam.transform.InverseTransformPoint(worldTarget);
            cappedScreenPos = new Vector3(
                Mathf.Clamp(screenPos.x, 0, Screen.width),
                Mathf.Clamp(screenPos.y, 0, Screen.height),
                screenPos.z);
            viewPortPos = MainCam.WorldToViewportPoint(worldTarget);
        }

        private void UpdateIsInViewPort() {
            IsOffScreen = screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;
            IsBehindPlayer = targetRelToCam.z < 0;
            _isInViewPort = !IsOffScreen && !IsBehindPlayer;
            if (wasOutOfView && IsInViewPort) {
                OnEnterViewPort.Invoke(); // likely need to be careful about the invoking order here
                wasOutOfView = false;
            }
            else if (!wasOutOfView && !IsInViewPort) {
                OnExitViewport.Invoke();
                wasOutOfView = true;
            }
        }
        #endregion
    }
}
