using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace MoveToCode {
    public abstract class ARTrackBehavior : MonoBehaviour {
        #region members
        List<Color> originalColors;
        List<MeshRenderer> _meshRenderers;
        List<MeshRenderer> meshRenderers {
            get {
                if (_meshRenderers == null) {
                    _meshRenderers = new List<MeshRenderer>();
                    foreach (Transform child in transform) {
                        _meshRenderers.AddRange(child.GetComponentsInChildren<MeshRenderer>());
                    }
                    MeshRenderer myRend = GetComponent<MeshRenderer>();
                    if (myRend != null) {
                        _meshRenderers.Add(myRend);
                    }
                    originalColors = new List<Color>();
                    foreach (MeshRenderer rend in _meshRenderers) {
                        originalColors.Add(rend.material.color);
                    }
                }
                return _meshRenderers;
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
        public void UpdateBehaviorCommon(ARTrackedImage img) {
            if (img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) {
                PulseAlphaMeshes();
            }
            else {
                ResetMeshAlphas();
            }
            UpdateBehaviorSpecific(img);
        }

        #endregion

        #region protected
        protected abstract void UpdateBehaviorSpecific(ARTrackedImage img);
        protected void PulseAlphaMeshes() {
            foreach (MeshRenderer rend in meshRenderers) {
                Color c = rend.material.color;
                c.a = CalculateAlpha(c.a);
                rend.material.color = c;
            }
        }
        protected void ResetMeshAlphas() {
            for (int i = 0; i < meshRenderers.Count; i++) {
                Color c = meshRenderers[i].material.color;
                c.a = originalColors[i].a;
                meshRenderers[i].material.color = c;
            }
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
            ResetMeshAlphas();
        }
        #endregion
    }
}
