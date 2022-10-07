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
                    _c = GetComponent<Collider>();
                }
                return _c;
            }
        }
        #endregion

        #region unity
        protected override void OnEnable() {
            base.OnEnable();
            OnClick.AddListener(OnHitHand.Invoke);
        }
        protected override void OnDisable() {
            base.OnDisable();
            OnClick.RemoveListener(OnHitHand.Invoke);
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
