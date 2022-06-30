using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace MoveToCode {
    public class GoalTrackBeh : ARTrackBehavior {
        #region members
        #endregion

        #region unity
        #endregion

        #region public
        #endregion

        #region private
        protected override void UpdateBehaviorSpecific(ARTrackedImage img) {
            if (img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking && !MazeManagerInstance.IsLocked) {
                transform.position = img.transform.position;
                transform.rotation = Quaternion.Euler(0, img.transform.rotation.eulerAngles.y, 0);
            }
        }
        #endregion
    }
}
