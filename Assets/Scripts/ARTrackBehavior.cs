using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

namespace MoveToCode {
    public abstract class ARTrackBehavior : MonoBehaviour {
        #region members
        Color OrigColor {
            get; set;
        }
        MeshRenderer _meshRenderer;
        MeshRenderer MeshRend {
            get {
                if (_meshRenderer == null) {
                    _meshRenderer = GetComponent<MeshRenderer>();
                    if (_meshRenderer != null) {
                        OrigColor = _meshRenderer.material.color;
                    }
                    else {
                        Debug.LogError("No renderer on tracked object " + transform.name);
                    }
                }
                return _meshRenderer;
            }
        }
        float alphaFloor = 0.2f, alphaCeil = 1.0f, alphaRatePerSec = 1.5f; // 1.5f per sec
        bool alphaRaising = true;

        ARTrackingManager aRTrackingManager;
        protected ARTrackingManager ARTrackingManagerInstance {
            get {
                if (aRTrackingManager == null) {
                    aRTrackingManager = ARTrackingManager.instance;
                }
                return aRTrackingManager;
            }
        }
        TrackingIndicator _trackingIndicator;
        TrackingIndicator TrackingIndicator {
            get {
                if (_trackingIndicator == null) {
                    _trackingIndicator = GetComponentInChildren<TrackingIndicator>();
                }
                return _trackingIndicator;
            }
        }

        public UnityEvent OnImgStartedTracking, OnImgStoppedTracking;
        bool isTracking = false;
        bool hasBeenInitialized = false;
        public bool HasBeenTracked = false; // indicates a piece is within a reasonable area

        public bool IsTracking {
            get {
                return isTracking;
            }
        }

        Material _outlineMat;
        Material OutlineMaterial {
            get {
                if (_outlineMat == null) {
                    _outlineMat = Resources.Load<Material>(ResourcePathConstants.OutlineMazePieceMaterial);
                }
                return _outlineMat;
            }
        }
        MeshOutline mo;
        MeshOutline MeshOut {
            get {
                if (mo == null) {
                    mo = GetComponent<MeshOutline>();
                    if (mo == null) {
                        mo = gameObject.AddComponent<MeshOutline>();
                    }
                    mo.OutlineMaterial = OutlineMaterial;
                    mo.OutlineWidth = 0.01f;
                    mo.enabled = false;
                }
                return mo;
            }
        }
        #endregion

        #region unity
        private void OnEnable() {
            if (!hasBeenInitialized) {
                TrackingIndicator.TurnOff();
                hasBeenInitialized = true;
            }
#if UNITY_EDITOR
            HasBeenTracked = true; // pretend it was tracked because we don't have real paper in the editor
#endif
        }

        #endregion

        #region public
        public void UpdateBehavior(ARTrackedImage img) {
            if (ARTrackingManagerInstance.IsTracking && img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) {
                HasBeenTracked = true;
                OnTrackingNow();
            }
            else {
                OnNotTracking();
            }
            UpdateBehaviorSpecific(img);
        }

        private void OnNotTracking() {
            ResetMeshAlpha();
            LowMeshAlpha();
            MeshOut.enabled = false;
            if (isTracking) {
                isTracking = false;
                TrackingIndicator.TurnOff();
                OnImgStoppedTracking.Invoke();
            }

        }

        private void OnTrackingNow() {
            //PulseAlphaMesh();
            HigherMeshAlpha();
            MeshOut.enabled = true;
            if (!isTracking) {
                isTracking = true;
                TrackingIndicator.TurnOn();
                OnImgStartedTracking.Invoke();
            }
        }

        #endregion

        #region protected
        protected abstract void UpdateBehaviorSpecific(ARTrackedImage img);

        protected void PulseAlphaMesh() {
            Color c = MeshRend.material.color;
            c.a = CalculateAlpha(c.a);
            MeshRend.material.color = c;
        }
        protected void ResetMeshAlpha() {
            MeshRend.material.color = OrigColor; // TODO make this more efficient/actually fix it -> meh no
        }

        void LowMeshAlpha(){
            Color c = MeshRend.material.color;
            c.a = 100;
            MeshRend.material.color = c;
        }

        void HigherMeshAlpha(){
            Color c = MeshRend.material.color;
            c.a = 170;
            MeshRend.material.color = c;
        }
        protected float CalculateAlpha(float curAlpha) {
            float tmpA = (curAlpha - alphaFloor) / (alphaCeil - alphaFloor); // normalize alpha
            float alphaRate = alphaRatePerSec * Time.deltaTime; // alpha rate normalized to frame rate
            tmpA = alphaRaising ? tmpA + alphaRate : tmpA - alphaRate; // make it go up or down
            if (tmpA > 1.0f) {
                tmpA = 1;
                alphaRaising = false;
            }
            else if (tmpA < 0.0f) {
                tmpA = 0;
                alphaRaising = true;
            }
            tmpA = tmpA * (alphaCeil - alphaFloor) + alphaFloor; // unnormalize alpha
            return tmpA;
        }
        #endregion

        #region private
        #endregion
    }
}
