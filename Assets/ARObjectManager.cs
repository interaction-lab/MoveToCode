using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace MoveToCode {
    public class ARObjectManager : MonoBehaviour {
        #region members
        Dictionary<string, GameObject> trackedObjDict;
        Dictionary<string, GameObject> TrackedObjDict {
            get {
                if (trackedObjDict == null) {
                    trackedObjDict = new Dictionary<string, GameObject>();
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
            TrackedObjDict[img.ImgName()].transform.rotation = Quaternion.Euler(0, img.transform.rotation.eulerAngles.y, 0); // need to keep level with the ground
        }

        private void ImageAdded(ARTrackedImage img) {
            if (!TrackedObjDict.ContainsKey(img.ImgName())) {
                TrackedObjDict.Add(img.ImgName(), Instantiate(ResourcePathConstants.mazeObjectDict[img.ImgName()]) as GameObject);
                TrackedObjDict[img.ImgName()].transform.position = img.transform.position;
                TrackedObjDict[img.ImgName()].transform.SetParent(img.transform); // keeps tracking position
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
