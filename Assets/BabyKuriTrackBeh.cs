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
        #endregion

        #region unity
        #endregion

        #region public
        public override void UpdateBehavior(ARTrackedImage img) {
            if (!interpreter.CodeIsRunning()) {
                transform.position = img.transform.position;
                transform.rotation = Quaternion.Euler(0, img.transform.rotation.y, 0);
            }
        }
        #endregion

        #region private
        #endregion
    }
}
