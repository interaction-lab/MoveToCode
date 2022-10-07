using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class KuriArms : Singleton<KuriArms> {
        #region members
        Animator anim;
        public Animator ArmAnimator {
            get {
                if (anim == null) {
                    anim = GetComponent<Animator>();
                }
                return anim;
            }
        }

        TargetIKObject _leftIKTarget, _rightIKTarget;
        public TargetIKObject LeftIKTarget {
            get {
                if (_leftIKTarget == null) {
                    _leftIKTarget = TutorKuriManager.instance.KController.IkObjLeft;
                }
                return _leftIKTarget;
            }
        }
        public TargetIKObject RightIKTarget {
            get {
                if (_rightIKTarget == null) {
                    _rightIKTarget = TutorKuriManager.instance.KController.IkObjRight;
                }
                return _rightIKTarget;
            }
        }
        #endregion

        #region unity
        #endregion

        #region public
        public void SetHandCollider(bool b, bool rightHand) {
            if (rightHand) {
                RightIKTarget.SetCollider(b);
            }
            else {
                LeftIKTarget.SetCollider(b);
            }
        }
        #endregion

        #region private
        #endregion
    }
}
