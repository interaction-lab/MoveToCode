using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class TrackingIndicator : MonoBehaviour {
        #region members
        MeshRenderer meshRenderer;
        MeshRenderer MyRenderer {
            get {
                if (meshRenderer == null) {
                    meshRenderer = GetComponent<MeshRenderer>();
                }
                return meshRenderer;
            }
        }
        #endregion
        #region unity
        #endregion
        #region public
        public void TurnOff() {
            MyRenderer.enabled = false;
        }
        public void TurnOn() {
            MyRenderer.enabled = true;
        }
        #endregion
        #region private
        #endregion
    }
}