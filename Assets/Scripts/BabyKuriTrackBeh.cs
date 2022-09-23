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
                    _bktransformmanager = BKM.GetComponent<BabyKuriTransformManager>();
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
        #endregion

        #region protected
        protected override void UpdateBehaviorSpecific(ARTrackedImage img) {
            if (ARTrackingManagerInstance.IsTracking && img.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) {
                transform.position = img.transform.position;
                transform.rotation = Quaternion.Euler(0, (img.transform.rotation.eulerAngles.y) % 360, 0);
                BKTransformManager.KuriPos = transform.position;
                BKTransformManager.KuriRot = transform.rotation;
            }
        }
        #endregion
    }
}
