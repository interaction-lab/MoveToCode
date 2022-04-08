using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MazeGoal : MonoBehaviour {
        #region members
        Collider _collider;
        Collider MyCollider {
            get {
                if (_collider == null) {
                    _collider = GetComponent<Collider>();
                }
                return _collider;
            }
        }
        #endregion

        #region unity
        private void OnEnable() {
            MyCollider.enabled = true;
            MyCollider.isTrigger = true;
        }

        private void OnDisable() {
            MyCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.name == "BKBody") {
                KuriTextManager.instance.Addline("You win!");
            }
        }
        #endregion

        #region public
        #endregion

        #region private
        #endregion
    }
}
