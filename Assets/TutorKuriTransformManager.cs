using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class TutorKuriTransformManager : MonoBehaviour {
        #region members
        public Vector3 Position {
            get {
                return transform.position;
            }
            set {
                transform.position = value;
            }
        }
        public Quaternion Rotation {
            get {
                return transform.rotation;
            }
            set {
                transform.rotation = value;
            }
        }
        public Transform MyTransform {
            get {
                return transform;
            }
        }
        #endregion
        #region unity
        #endregion
        #region public
        #endregion
        #region private
        #endregion
    }
}
