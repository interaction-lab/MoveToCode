using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.Events;

namespace MoveToCode {
    public class UntilInteract : MonoBehaviour {
        #region members
        public UnityEvent OnInteract = new UnityEvent();
        ManipulationHandler mh;
        Interactable interactable;
        #endregion

        #region unity
        private void OnEnable() {
            mh = GetComponent<ManipulationHandler>();
            interactable = GetComponent<Interactable>();
            if (mh != null) {
                mh.OnManipulationStarted.AddListener(InvokeInteract);
            }
            if (interactable != null) {
                interactable.OnClick.AddListener(InvokeInteract);
            }
        }

        private void InvokeInteract() {
            OnInteract.Invoke();
        }

        private void InvokeInteract(ManipulationEventData arg0) {
            OnInteract.Invoke();
        }
        #endregion

        #region public
        #endregion

        #region private

        #endregion
    }
}
