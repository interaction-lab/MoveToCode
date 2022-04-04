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
        ARTrackedImageManager ARTrackedImageManagerInstance {
            get {
                if (arTrackedImageManager == null) {
                    arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
                }
                return arTrackedImageManager;
            }
        }

        ARTrackingManager arTrackingManager;
        ARTrackingManager ARTrackingManagerInstance {
            get {
                if (arTrackingManager == null) {
                    arTrackingManager = ARTrackingManager.instance;
                }
                return arTrackingManager;
            }
        }

        MazeManager mazeManager;
        MazeManager MazeManagerInstance {
            get {
                if (mazeManager == null) {
                    mazeManager = MazeManager.instance;
                }
                return mazeManager;
            }
        }
        #endregion

        #region unity
        private void Awake() {
            ARTrackedImageManagerInstance.maxNumberOfMovingImages = ResourcePathConstants.mazeObjectDict.Count;
        }
        private void OnEnable() {
            ARTrackedImageManagerInstance.trackedImagesChanged += ImageChanged;
        }
        private void OnDisable() {
            ARTrackedImageManagerInstance.trackedImagesChanged -= ImageChanged;
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
            if (ARTrackingManagerInstance.IsTracking) {
                TrackedObjDict[img.ImgName()].UpdateBehavior(img);
            }
        }

        public void ImageAdded(ARTrackedImage img) {
            if (!TrackedObjDict.ContainsKey(img.ImgName())) {
                if (img.ImgName() != ResourcePathConstants.kuri_start) {
                    GameObject go = MazeManagerInstance.GetMazeObject(img.ImgName());
                    TrackedObjDict.Add(img.ImgName(), go.GetComponent<ARTrackBehavior>());
                }
                else {
                    TrackedObjDict.Add(img.ImgName(), BabyKuriManager.instance.gameObject.GetComponent<ARTrackBehavior>()); // special case for baby Kuri
                }
            }
            ImageUpdated(img);
        }

        #endregion
    }
}
