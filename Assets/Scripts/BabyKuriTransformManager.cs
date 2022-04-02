using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class BabyKuriTransformManager : MonoBehaviour {
        #region members
        public Vector3 OriginalPosition { get; private set; }
        public Quaternion OriginalRotation { get; private set; }
        public Color OriginalColor { get; private set; }

        public Vector3 KuriPos {
            get {
                return transform.position;
            }
            set {
                transform.position = value;
            }
        }
        public Quaternion KuriRot {
            get {
                return transform.rotation;
            }
            set {
                transform.rotation = value;
            }
        }
        public Vector3 Forward {
            get {
                return transform.forward * -1f;
            }
        }
        private bool _origstateset = false;
        public bool OrigStateSet {
            get {
                return _origstateset;
            }
        }

        public Vector3 Up {
            get {
                return transform.up;
            }
        }

        private MeshRenderer _bodyPlateRend;
        public MeshRenderer BodyPlateRend {
            get {
                if (_bodyPlateRend == null) {
                    _bodyPlateRend = GetComponentInChildren<MeshRenderer>();
                }
                return _bodyPlateRend;
            }
        }
        #endregion

        #region unity
        #endregion

        #region public
        public void SetOriginalState() {
            OriginalPosition = KuriPos;
            OriginalRotation = KuriRot;
            OriginalColor = BodyPlateRend.material.color;
            _origstateset = true;
        }
        public void ResetToOriginalState(){
            if(!OrigStateSet){
                SetOriginalState();
            }
            transform.position = OriginalPosition;
            transform.rotation = OriginalRotation;
            BodyPlateRend.material.color = OriginalColor;
        }
        #endregion

        #region private
        #endregion
    }
}