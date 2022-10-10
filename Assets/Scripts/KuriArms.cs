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

        Transform rHand, lHand, rShoulder, lShoulder;

        public Transform RShoulder {
            get {
                if (rShoulder == null)
                    rShoulder = transform.GetChild(0).GetChild(2);
                return rShoulder;
            }
        }
        public Transform RHand {
            get {
                if (rHand == null)
                    rHand = RShoulder.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
                return rHand;
            }
        }
        public Transform LShoulder {
            get {
                if (lShoulder == null) {
                    lShoulder = transform.GetChild(1).GetChild(2);
                }
                return lShoulder;
            }
        }
        public Transform LHand {
            get {
                if (lHand == null) {
                    lHand = LShoulder.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
                }
                return lHand;
            }
        }

        public PulseMeshRend RPulseMeshRend {
            get {
                return RHand.GetComponent<PulseMeshRend>();
            }
        }
        public PulseMeshRend LPulseMeshRend {
            get {
                return LHand.GetComponent<PulseMeshRend>();
            }
        }

        VirtualKuriAudio audioManager;
        public VirtualKuriAudio AudioManager {
            get {
                if (audioManager == null) {
                    audioManager = TutorKuriManager.instance.KuriAudio; ;
                }
                return audioManager;
            }
        }

        #endregion

        #region unity
        private void OnEnable() {
#if !UNITY_EDITOR
            // turn off the MeshRenderer of both IK objetcs
            RightIKTarget.GetComponent<MeshRenderer>().enabled = false;
            LeftIKTarget.GetComponent<MeshRenderer>().enabled = false;
#endif
        }
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

        public void PlayClap() {
            AudioManager.PlayClap();
        }
        #endregion

        #region private
        #endregion
    }
}
