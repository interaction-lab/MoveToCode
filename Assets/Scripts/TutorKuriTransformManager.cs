using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class TutorKuriTransformManager : Singleton<TutorKuriTransformManager> {
        #region members
        public Transform _bodyTransform, _headTransform;
        Transform _originT;
        public Transform OriginT {
            get {
                if (_originT == null) {
                    _originT = _bodyTransform;
                }
                return _originT;
            }
        }
        public Vector3 Position {
            get {
                return transform.position;
            }
            set {
                transform.position = value;
            }
        }
        public Quaternion BodyRotation {
            get {
                return _bodyTransform.rotation;
            }
            set {
                _bodyTransform.rotation = value;
            }
        }

        public Quaternion Rotation { // default to body rotation, in compliance with NRI-SVET (aka I'm lazy)
            get {
                return BodyRotation;
            }
            set {
                BodyRotation = value;
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

        public Vector3 Forward {
            get {
                return _bodyTransform.forward;
            }
        }

        float _groundYCord = 0f;
        public virtual float GroundYCord {
            get {
                return _groundYCord + 0.00001f; // just off the ground to deal with issues with floating point
            }
            set {
                _groundYCord = value;
            }
        }
        #endregion
        #region unity
        private void Update() {
            PlaceKuriOnGround();
        }

        private void PlaceKuriOnGround() {
            Vector3 groundPosition = Position;
            groundPosition.y = GroundYCord;
            Position = groundPosition;
        }

        private void FixedUpdate() {
            // raycast down to find ground
            RaycastHit hit;
            // make raycast only hit Spatial Awareness Layers
            int layerMask = 1 << LayerMask.NameToLayer("Spatial Awareness");
            float lastGroundy = GroundYCord;
            if (Physics.Raycast(OriginT.position + (Vector3.up * 0.1f), Vector3.down, out hit, 10, layerMask)) {
                if (hit.point.y < lastGroundy) {
                    GroundYCord = hit.point.y;
                }
            }
            else {
                // raycast up to find ground
                if (Physics.Raycast(OriginT.position - (Vector3.up * 0.1f), Vector3.up, out hit, 10, layerMask)) {
                    if (hit.point.y < lastGroundy) {
                        GroundYCord = hit.point.y;
                    }
                }
            }
        }
        #endregion
        #region public
        public bool IsAtPosition(Vector3 position) {
            return Vector3.Distance(position, Position) < 0.01f;
        }

        public bool IsWithinHeadPanConstraints() {
            float headYAngle = ExtensionMethods.NormalizeAngle(HeadRotation.eulerAngles.y);
            float bodyYAngle = ExtensionMethods.NormalizeAngle(BodyRotation.eulerAngles.y);
            float angleDiff = Mathf.Abs(ExtensionMethods.NormalizedAngleSubtract(headYAngle, bodyYAngle));
            return angleDiff <= KuriContraints.HeadPanDeg;
        }
        #endregion
        #region private
        #endregion
    }
}
