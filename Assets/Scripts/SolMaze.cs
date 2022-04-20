using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SolMaze : MonoBehaviour {
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

        MazeGraph mazeGraph;
        public MazeGraph MyMazeGraph {
            get {
                if (mazeGraph == null) {
                    mazeGraph = new MazeGraph(BKMazePiece);
                }
                return mazeGraph;
            }
        }
        #endregion

        #region unity
        #endregion

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
