using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

namespace MoveToCode {
    public abstract class ARTrackBehavior : MonoBehaviour {
        #region members
        Color OrigColor { get; set; }
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
        ARTrackingManager ARTrackingManagerInstance {
            get {
                if (aRTrackingManager == null) {
                    aRTrackingManager = ARTrackingManager.instance;
                }
                return aRTrackingManager;
            }
        }

        public UnityEvent OnImgStartedTracking, OnImgStoppedTracking;
        bool isTracking = false;
        #endregion

        #region unity
        private void OnEnable() {
            ARTrackingManagerInstance.OnTrackingEnded.AddListener(OnTrackingEnded);
        }

        private void OnDisable() {
            ARTrackingManagerInstance.OnTrackingEnded.RemoveListener(OnTrackingEnded);
        }
        #endregion

        #region public
        public void UpdateBehavior(ARTrackedImage img) {
            if (img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) {
                PulseAlphaMesh();
                if (!isTracking) {
                    OnImgStartedTracking.Invoke();
                    isTracking = true;
                }
            }
            else {
                ResetMeshAlpha();
                if (isTracking) {
                    OnImgStoppedTracking.Invoke();
                    isTracking = false;
                }
            }
            UpdateBehaviorSpecific(img);
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
            MeshRend.material.color = OrigColor; // TODO make this more efficient/actually fix it
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
        private void OnTrackingEnded() {
            ResetMeshAlpha();
        }
        #endregion
    }
}
