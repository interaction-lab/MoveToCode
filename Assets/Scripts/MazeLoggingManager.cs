using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MazeLoggingManager : Singleton<MazeLoggingManager> {
        #region members
        MazeManager _mm;
        MazeManager MazeManagerInstance {
            get {
                if (_mm == null) {
                    _mm = MazeManager.instance;
                }
                return _mm;
            }
        }
        LoggingManager lm;
        LoggingManager LoggingManagerInstance {
            get {
                if (lm == null) {
                    lm = LoggingManager.instance;
                }
                return lm;
            }
        }
        #endregion

        #region unity
        #endregion

        #region public
        public void LogMaze() {
            StartCoroutine(LogMazeCoroutine());
        }
        #endregion

        #region private
        bool loggedThisFrame = false;
        IEnumerator LogMazeCoroutine() {
            yield return new WaitForEndOfFrame();
            if (loggedThisFrame) {
                yield break;
            }
            loggedThisFrame = true;
            LoggingManagerInstance.UpdateLogColumn(MazeManager.mazeLogCol, MazeManagerInstance.MyMazeGraph.ToString());
            LoggingManagerInstance.UpdateLogColumn(MazeManager.containsSolCol, MazeManagerInstance.IsSameAsSolutionMaze() ? "1" : "0");
            SolMazeCheckMark.instance.ToggleCheckMark(); // this is super hacky
            yield return new WaitForEndOfFrame();
            loggedThisFrame = false;
        }
        #endregion
    }
}
