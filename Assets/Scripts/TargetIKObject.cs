using Microsoft.MixedReality.Toolkit.UI;

using UnityEngine;
using UnityEngine.Events;

namespace MoveToCode {
    public class TargetIKObject : Interactable {
        #region members
        public bool IsRightArm = false;
        public UnityEvent OnHitHand = new UnityEvent();
        Collider _c;
        Collider Col {
            get {
                if (_c == null) {
                    if (IsRightArm) {
                        _c = KuriArms.instance.RHand.GetComponent<Collider>();
                    }
                    else {
                        _c = KuriArms.instance.LHand.GetComponent<Collider>();
                    }
                }
                return _c;
            }
        }
        Interactable _i;
        Interactable Interact {
            get {
                if (_i == null) {
                    _i = Col.GetComponent<Interactable>();
                }
                return _i;
            }
        }
        #endregion

        #region unity
        protected override void OnEnable() {
            base.OnEnable();
            Interact.OnClick.AddListener(OnHitHand.Invoke);
        }
        protected override void OnDisable() {
            base.OnDisable();
            Interact.OnClick.RemoveListener(OnHitHand.Invoke);
        }
        #endregion

        #region public
        public void SetCollider(bool b) {
            enabled = b;
            Col.enabled = b;
        }
        #endregion

        #region private
        #endregion
    }
}
