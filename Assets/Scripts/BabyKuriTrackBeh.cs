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

        ARTrackingManager _artrackingmanager;
        ARTrackingManager ARTrackingManagerInstance {
            get {
                if (_artrackingmanager == null) {
                    _artrackingmanager = ARTrackingManager.instance;
                }
                return _artrackingmanager;
            }
        }

        #endregion

        #region unity
        #endregion

        #region public
        public override void UpdateBehavior(ARTrackedImage img) {
            if (ARTrackingManagerInstance.IsTracking) {
                BKTransformManager.KuriPos = img.transform.position;
                // This needs to be rotated 180 degrees to match the paper arrow rotation
                BKTransformManager.KuriRot = Quaternion.Euler(0, (img.transform.rotation.eulerAngles.y + 180) % 360, 0);
                BKTransformManager.SetOriginalState();
            }
        }
        #endregion

        #region private
        #endregion
    }
}
