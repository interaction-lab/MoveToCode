using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MoveToCode {
    public class DisableParentGOButton : MonoBehaviour {
        Button button;
        private void Awake() {
            button = GetComponent<Button>();
            button.onClick.AddListener(delegate {
                DisableParentGO();
            });
        }
        void DisableParentGO() {
            transform.parent.gameObject.SetActive(false);
        }
    }


}