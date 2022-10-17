using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SolMazeManager : Singleton<SolMazeManager> {
        #region members
        public static string solutionMazeCol = "SolutionMaze", exerciseNameCol = "ExerciseName";

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
                    foreach (SolMaze solMaze in _allSolMazes) {
                        solMaze.gameObject.SetActive(false);
                    }
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
        SwitchModeButton smb;
        SwitchModeButton ModeButton {
            get {
                if (smb == null) {
                    smb = SwitchModeButton.instance;
                }
                return smb;
            }
        }
        #endregion

        #region unity
        private void OnEnable() {
            if (!hasBeenInitialized) {
                LoggingManagerInstance.AddLogColumn(solutionMazeCol, "");
                LoggingManagerInstance.AddLogColumn(exerciseNameCol, "");
                hasBeenInitialized = true;
                CurActiveSolMaze.gameObject.SetActive(true);
                ExerciseManager.instance.OnCyleNewExercise.AddListener(OnCyleNewExercise);
                ModeButton.OnSwitchToMazeBuildingMode.AddListener(OnSwitchToMazeBuildingMode);
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
        int exerciseNumLogged = -1;
        IEnumerator LogMazeCoroutine() {
            yield return null;
            yield return null; // hacky but fixes the solution graph initializing order
            if (exerciseNumLogged < ExerciseManager.instance.curExercisePos) { // allows maze to log as soon as a new exercise is actually moved to
                Debug.Log(CurActiveSolMaze.MyMazeGraph.ToString());
                LoggingManagerInstance.UpdateLogColumn(solutionMazeCol, CurActiveSolMaze.MyMazeGraph.ToString());
                LoggingManagerInstance.UpdateLogColumn(exerciseNameCol, CurActiveSolMaze.gameObject.name);
                exerciseNumLogged = ExerciseManager.instance.curExercisePos;
            }
        }

        void OnSwitchToMazeBuildingMode() {
            LogMaze();
        }

        bool freePlayIsActive = false;
        private void OnCyleNewExercise() {
            int curIndex = AllSolMazes.IndexOf(CurActiveSolMaze);
            curActiveSolMaze.gameObject.SetActive(false);

            if (curIndex + 2 >= AllSolMazes.Count && !freePlayIsActive) {
                // we are at freeplay
                SolMazeCheckMark.instance.SetFreePlayText();
                freePlayIsActive = true;
            }
            // if so that we activate the final sol goal maze
            if (curIndex + 1 < AllSolMazes.Count) {
                curActiveSolMaze = AllSolMazes[curIndex + 1];
                curActiveSolMaze.gameObject.SetActive(true);
                SolMazeCheckMark.instance.ToggleCheckMark();
            }
        }
        #endregion
    }
}
