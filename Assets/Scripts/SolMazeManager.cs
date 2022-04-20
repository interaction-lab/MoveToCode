using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SolMazeManager : Singleton<SolMazeManager> {
        #region members
        MazePiece bkMazePiece;
        public static string solutionMazeCol = "SolutionMaze";
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
        LoggingManager _loggingManager;
        LoggingManager LoggingManagerInstance {
            get {
                if (_loggingManager == null) {
                    _loggingManager = LoggingManager.instance;
                }
                return _loggingManager;
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

        bool hasBeenInitialized = false;
        #endregion

        #region unity
        private void OnEnable() {
            if (!hasBeenInitialized) {
                LoggingManagerInstance.AddLogColumn(solutionMazeCol, "");
                hasBeenInitialized = true;
            }
        }
        #endregion
        // need to turn off all the connectors that are not currently in contact with something
        // or I could just turn them all off and not worry about it
        #region public
        public void LogMaze() {
            // wait one frame to allow for collisions to be set up
            StartCoroutine(LogMazeCoroutine());
        }
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
        IEnumerator LogMazeCoroutine() {
            yield return new WaitForEndOfFrame();
            LoggingManagerInstance.UpdateLogColumn(solutionMazeCol, MyMazeGraph.ToString());
        }
        #endregion
    }
}
