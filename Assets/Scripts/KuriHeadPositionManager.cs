using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class KuriHeadPositionManager : Singleton<KuriHeadPositionManager> {
        #region members
        public Vector3 HeadPosition {
            get {
                return transform.position;
            }
        }
        public Transform HeadPan, HeadTilt, EyeLids; // vestigial

        public Quaternion HeadRotation {
            get {
                return transform.rotation;
            }
            set {
                transform.rotation = value;
            }
        }
        public Transform OriginT{
            get{
                return transform;
            }
        }

        public float AboveHeadOffset = 0.5f;
        #endregion
        #region unity
        #endregion
        #region public
        #endregion
        #region private
        #endregion
    }
}
