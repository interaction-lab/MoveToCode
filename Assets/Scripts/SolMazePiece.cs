using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SolMazePiece : MazePiece {
        #region members
        #endregion

        #region unity
        void Awake() {
            IsAnchored = false; // super hacky workaround for MazePiece adding the TrashButtons
        }
        #endregion

        #region public
        #endregion

        #region protected
        protected override void SetUpOnEnable() {
            // change my layer to default
            SetUpLayerMask(LayerMask.NameToLayer(LayerMaskConstants.DEFAULT)); // avoids raycast issues with MazePieces
        }
        protected override void RunOnDisable() {
            // do nothing
        }
        #endregion

        #region private
        #endregion
    }
}
