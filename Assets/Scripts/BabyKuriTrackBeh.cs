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
        BabyKuriTransformManager _bktransformmanager;
        BabyKuriTransformManager BKTransformManager {
            get {
                if (_bktransformmanager == null) {
                    _bktransformmanager = GetComponent<BabyKuriTransformManager>();
                }
                return _bktransformmanager;
            }
        }

        #endregion

        #region unity
        #endregion

        #region public
        public override void UpdateBehavior(ARTrackedImage img) {
            if (!interpreter.IsInResetState()) {
                BKTransformManager.KuriPos = img.transform.position;
                // This needs to be rotated 180 degrees to match the paper arrow rotation
                BKTransformManager.KuriRot = Quaternion.Euler(0, img.transform.rotation.y + 180, 0);
                BKTransformManager.SetOriginalState();
            }
        }
        #endregion

        #region private
        #endregion
    }
}
