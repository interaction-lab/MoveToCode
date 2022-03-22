using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace MoveToCode {
    public class ARObjectManager : MonoBehaviour {
        #region members
        public GameObject trackable;
        ARTrackedImageManager arTrackedImageManager;
        #endregion

        #region unity
        private void Awake() {

        }
        private void OnEnable() {
            KuriTextManager.instance.Addline("enable");
            GetARTrackedImageManager().trackedImagesChanged += ImageChanged;
        }
        private void OnDisable() {
            GetARTrackedImageManager().trackedImagesChanged -= ImageChanged;
        }
        #endregion

        #region public

        #endregion

        #region private
        private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs) {
            foreach (ARTrackedImage img in eventArgs.added) {
                trackable.transform.position = img.transform.position;
                KuriTextManager.instance.Addline("add");
            }
            foreach (ARTrackedImage img in eventArgs.updated) {
                trackable.transform.position = img.transform.position;
                KuriTextManager.instance.Addline(img.transform.name);
            }

        }

        private ARTrackedImageManager GetARTrackedImageManager() {
            if (arTrackedImageManager == null) {
                arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
            }
            return arTrackedImageManager;
        }
        #endregion
    }
}
