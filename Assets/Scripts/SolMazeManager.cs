using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SolMazeManager : Singleton<SolMazeManager> {
        #region members
        MazePiece bkMazePiece;

        public MazePiece BKMazePiece {
            get {
                if (bkMazePiece == null) {
                    foreach (Transform t in transform) {
                        if (t.GetComponent<MazeBabyKuri>() != null) {
                            bkMazePiece = t.GetComponent<MazePiece>();
                        }
                    }
                }
                return bkMazePiece;
            }
        }
        #endregion

        #region unity
        #endregion
        // need to turn off all the connectors that are not currently in contact with something
        // or I could just turn them all off and not worry about it
        #region public
        public void ReleasePieces() {
            foreach (Transform child in transform) {
                MazePiece mazePiece = child.GetComponent<MazePiece>();
                if (mazePiece != null) {
                    mazePiece.UnAnchor();
                }
            }
        }

        public void SnapPiecesTogether() {
            BKMazePiece.SnapConnections();
        }
        #endregion

        #region private
        #endregion
    }
}
