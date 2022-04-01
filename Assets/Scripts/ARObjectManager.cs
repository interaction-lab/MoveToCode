using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace MoveToCode {
    public class ARObjectManager : MonoBehaviour {
        #region members
        Dictionary<string, ARTrackBehavior> trackedObjDict;
        Dictionary<string, ARTrackBehavior> TrackedObjDict {
            get {
                if (trackedObjDict == null) {
                    trackedObjDict = new Dictionary<string, ARTrackBehavior>();
                }
                return trackedObjDict;
            }
        }
        ARTrackedImageManager arTrackedImageManager;
        #endregion

        #region unity
        private void Awake() {
            GetARTrackedImageManager().maxNumberOfMovingImages = ResourcePathConstants.mazeObjectDict.Count;
        }
        private void OnEnable() {
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
                KuriTextManager.instance.Addline(img.ImgName());
                ImageAdded(img);
            }
            foreach (ARTrackedImage img in eventArgs.updated) {
                ImageUpdated(img);
            }
            foreach (ARTrackedImage img in eventArgs.removed) {
                // put img remove events
            }
        }


        private void ImageUpdated(ARTrackedImage img) {
            TrackedObjDict[img.ImgName()].UpdateBehavior(img);
        }

        private void ImageAdded(ARTrackedImage img) {
            if (!TrackedObjDict.ContainsKey(img.ImgName())) {
                if (img.ImgName() != ResourcePathConstants.kuri_start) {
                    TrackedObjDict.Add(img.ImgName(), Instantiate(ResourcePathConstants.mazeObjectDict[img.ImgName()]).GetComponent<ARTrackBehavior>());
                }
                else {
                    TrackedObjDict.Add(img.ImgName(), BabyKuriManager.instance.gameObject.GetComponent<ARTrackBehavior>()); // special case for baby Kuri
                }
            }
            ImageUpdated(img);
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
