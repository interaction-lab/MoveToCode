using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace MoveToCode {
    public class StopTrackingBox : MonoBehaviour {
        #region members
        Interactable _interactable;
        Interactable MyInteractable {
            get {
                if (_interactable == null) {
                    _interactable = GetComponent<Interactable>();
                }
                return _interactable;
            }
        }

        UnityEngine.UI.Button _button;
        UnityEngine.UI.Button MyButton {
            get {
                if (_button == null) {
                    _button = transform.parent.GetComponent<UnityEngine.UI.Button>();
                }
                return _button;
            }
        }

        #endregion

        #region unity
        private void Awake() {
            MyInteractable.OnClick.AddListener(Onclick);
        }

        private void Start() {
            ChangeMyColorToParent();
        }
        #endregion

        #region public
        #endregion

        #region private
        void Onclick() {
            MyButton.onClick.Invoke();
        }

        void ChangeMyColorToParent() {
            GetComponent<MeshRenderer>().material = transform.parent.parent.parent.GetComponent<MeshRenderer>().material;
        }
        #endregion
    }
}
