using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace MoveToCode {
    public class BabyKuriTrackBeh : ARTrackBehavior {
        #region members
        BabyKuriManager _bkm;
        BabyKuriManager BKM {
            get {
                if (_bkm == null) {
                    _bkm = BabyKuriManager.instance;
                }
                return _bkm;
            }
        }
        Interpreter _interpreter;
        Interpreter interpreter {
            get {
                if (_interpreter == null) {
                    _interpreter = Interpreter.instance;
                }
                return _interpreter;
            }
        }
        BabyVirtualKuriController _bvkc;
        BabyVirtualKuriController BVKC {
            get {
                if (_bvkc == null) {
                    _bvkc = BKM.kuriController;
                }
                return _bvkc;
            }
        }
        #endregion

        #region unity
        #endregion

        #region public
        public override void UpdateBehavior(ARTrackedImage img) {
            if (!interpreter.IsInResetState()) {
                transform.position = img.transform.position;
                // This needs to be rotated 180 degrees to match the paper arrow rotation
                transform.rotation = Quaternion.Euler(0, img.transform.rotation.y + 180, 0);
                BVKC.OriginalPosition = transform.position;
                BVKC.OriginalRotation = transform.rotation;
            }
        }
        #endregion

        #region private
        #endregion
    }
}
