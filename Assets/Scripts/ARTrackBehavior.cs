using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace MoveToCode {
    public abstract class ARTrackBehavior : MonoBehaviour {
        #region members
        #endregion

        #region unity
        #endregion

        #region public
        public abstract void UpdateBehavior(ARTrackedImage img);
        #endregion

        #region private
        #endregion
    }
}
