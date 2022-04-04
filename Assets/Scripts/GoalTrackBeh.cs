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
        public override void UpdateBehavior(ARTrackedImage img) {
            transform.position = img.transform.position;
            transform.rotation = Quaternion.Euler(0, img.transform.rotation.eulerAngles.y, 0);
        }
        #endregion

        #region private
        #endregion
    }
}
