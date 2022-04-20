using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SolMazeManager : Singleton<SolMazeManager> {
        #region members
        public static string solutionMazeCol = "SolutionMaze";

        LoggingManager _loggingManager;
        LoggingManager LoggingManagerInstance {
            get {
                if (_loggingManager == null) {
                    _loggingManager = LoggingManager.instance;
                }
                return _loggingManager;
            }
        }
        SolMaze curActiveSolMaze = null;
        public SolMaze CurActiveSolMaze {
            get {
                if (curActiveSolMaze == null) {
                    curActiveSolMaze = AllSolMazes[0];
                }
                return curActiveSolMaze;
            }
        }
        List<SolMaze> _allSolMazes;
        List<SolMaze> AllSolMazes {
            get {
                if (_allSolMazes == null) {
                    _allSolMazes = new List<SolMaze>();
                    _allSolMazes.AddRange(GetComponentsInChildren<SolMaze>(true));
                }
                return _allSolMazes;
            }
        }

        public MazeGraph ActiveSolMazeGraph {
            get {
                return CurActiveSolMaze.MyMazeGraph;
            }
        }

        bool hasBeenInitialized = false;
        #endregion

        #region unity
        private void OnEnable() {
            if (!hasBeenInitialized) {
                LoggingManagerInstance.AddLogColumn(solutionMazeCol, "");
                hasBeenInitialized = true;
                CurActiveSolMaze.gameObject.SetActive(true);
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
            curActiveSolMaze.ReleasePieces();
        }

        public void SnapPiecesTogether() {
            curActiveSolMaze.SnapPiecesTogether();
        }
        #endregion

        #region private
        IEnumerator LogMazeCoroutine() {
            yield return new WaitForEndOfFrame();
            LoggingManagerInstance.UpdateLogColumn(solutionMazeCol, curActiveSolMaze.MyMazeGraph.ToString());
        }
        #endregion
    }
}
