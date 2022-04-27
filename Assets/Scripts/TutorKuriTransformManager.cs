using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class TutorKuriTransformManager : MonoBehaviour {
        #region members
        public Transform _bodyTransform, _headTransform;
        public Vector3 Position {
            get {
                return transform.position;
            }
            set {
                transform.position = value;
            }
        }
        public Quaternion BodyRotation{
            get {
                return _bodyTransform.rotation;
            }
            set {
                _bodyTransform.rotation = value;
            }
        }
        public Quaternion HeadRotation {
            get {
                return _headTransform.rotation;
            }
            set {
               _headTransform.rotation = value;
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
        public bool IsAtPosition(Vector3 position) {
            return Vector3.Distance(position, Position) < 0.01f;
        }
        #endregion
        #region private
        #endregion
    }
}
