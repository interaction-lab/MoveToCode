using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.Events;

namespace MoveToCode {
    public class UntilInteract : MonoBehaviour {
        #region members
        public UnityEvent OnInteract = new UnityEvent();
        ManipulationHandler mh;
        Interactable interactable;
        MazePiece mp;
        #endregion

        #region unity
        private void OnEnable() {
            mh = GetComponent<ManipulationHandler>();
            interactable = GetComponent<Interactable>();
            mp = GetComponent<MazePiece>();
            if (mh != null) {
                mh.OnManipulationStarted.AddListener(InvokeInteract);
            }
            if (interactable != null) {
                interactable.OnClick.AddListener(InvokeInteract);
            }
            // need something for tracked objs, difficult because it is with a real world obj, might be able to do it with MazePiece events specifically
            if (mp != null) {
                // on new connection for this maze piece I think is the way to go
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
