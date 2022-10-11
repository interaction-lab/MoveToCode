using System;
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
        public Transform OriginT {
            get {
                return transform;
            }
        }

        public float AboveHeadOffset = 0.5f;
        bool initialized = false;
        Quaternion origHeadRotation;
        #endregion
        #region unity
        private void OnEnable() {
            if (!initialized) {
                origHeadRotation = HeadRotation;
                initialized = true;
            }
        }
        #endregion
        #region public
        public void ResetHead() {
            // set this relative to the body rotation
            Quaternion bodyRotation = TutorKuriTransformManager.instance.BodyRotation;
            HeadRotation = bodyRotation * origHeadRotation;
            // rotate head 180 degrees
            HeadRotation = Quaternion.Euler(HeadRotation.eulerAngles.x, HeadRotation.eulerAngles.y + 180, HeadRotation.eulerAngles.z);
        }
        #endregion
        #region private
        #endregion
    }
}
