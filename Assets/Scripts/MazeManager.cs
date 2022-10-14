using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;
using UnityEngine.Events;
using System.Collections;

namespace MoveToCode {
    public class MazeManager : Singleton<MazeManager> {
        #region members
        public UnityEvent OnMazeLocked, OnMazeUnlocked, OnBKGoalEnter, OnBKGoalExit;
        public static string mazeLogCol = "UserMaze", mazeLockCol = "mazeLock", containsSolCol = "containsSol";
        HashSet<Connection> populatedConnections = new HashSet<Connection>();
        Dictionary<MazeConnector, HashSet<MazeConnector>> connectRequests = new Dictionary<MazeConnector, HashSet<MazeConnector>>();
        BabyKuriManager _babyKuriManager;
        BabyKuriManager BabyKuriManagerInstance {
            get {
                if (_babyKuriManager == null) {
                    _babyKuriManager = BabyKuriManager.instance;
                }
                return _babyKuriManager;
            }
        }
        MazePiece bkMazePiece;
        public MazePiece BKMazePiece {
            get {
                if (bkMazePiece == null) {
                    foreach (Transform t in transform) {
                        if (t.GetComponent<StartingBKMazePiece>() != null) {
                            bkMazePiece = t.GetComponent<MazePiece>();
                        }
                    }
                }
                return bkMazePiece;
            }
        }
        BabyKuriTransformManager _bkTransformManager;
        BabyKuriTransformManager BKTransformManager {
            get {
                if (_bkTransformManager == null) {
                    _bkTransformManager = BabyKuriManagerInstance.GetComponent<BabyKuriTransformManager>();
                }
                return _bkTransformManager;
            }
        }
        SolMazeManager solMazeManager;
        SolMazeManager SolMazeManagerInstance {
            get {
                if (solMazeManager == null) {
                    solMazeManager = SolMazeManager.instance;
                }
                return solMazeManager;
            }
        }
        LoggingManager loggingManager;
        LoggingManager LoggingManagerInstance {
            get {
                if (loggingManager == null) {
                    loggingManager = LoggingManager.instance;
                }
                return loggingManager;
            }
        }
        SwitchModeButton smb;
        SwitchModeButton SMB {
            get {
                if (smb == null) {
                    smb = SwitchModeButton.instance;
                }
                return smb;
            }
        }
        public MazePiece BKCurrentMP = null;
        public bool IsLocked {
            get {
                return SMB.CurrentMode != SwitchModeButton.MODE.MazeBuilding;
            }
        }
        bool hasBeenInitialized = false;
        MazeGraph mazeGraph = null;
        public MazeGraph MyMazeGraph {
            get {
                if (mazeGraph == null) {
                    mazeGraph = new MazeGraph(BKMazePiece);
                }
                return mazeGraph;
            }
        }

        MazePiece _goalPiece;
        public MazePiece GoalMazePiece {
            get {
                if (_goalPiece == null) {
                    _goalPiece = GetComponentInChildren<MazeGoal>().GetComponent<MazePiece>();
                }
                return _goalPiece;
            }
        }
        public HashSet<MazePiece> _allMazePieces;
        public HashSet<MazePiece> AllMazePieces {
            get {
                if (_allMazePieces == null) {
                    _allMazePieces = new HashSet<MazePiece>();
                    foreach (MazePiece mp in GetComponentsInChildren<MazePiece>()) {
                        _allMazePieces.Add(mp);
                    }
                }
                return _allMazePieces;
            }
        }

        MazeLoggingManager mlm;
        MazeLoggingManager MLM {
            get {
                if (mlm == null) {
                    mlm = MazeLoggingManager.instance;
                }
                return mlm;
            }
        }
        #endregion
        #region unity
        private void OnEnable() {
            if (!hasBeenInitialized) {
                LoggingManagerInstance.AddLogColumn(mazeLogCol, "");
                LoggingManagerInstance.AddLogColumn(mazeLockCol, "");
                LoggingManagerInstance.AddLogColumn(containsSolCol, "");
                hasBeenInitialized = true;
            }
#if UNITY_EDITOR
            AddManipulationHandlersForUnityEditor();
#else
            MoveOutOfView(); // Move the maze out of view so that it doesn't get in the way of the user when deployed to a device
#endif
        }
        #endregion
        #region public
        public GameObject GetMazeObject(string name) {
            foreach (Transform child in transform) {
                if (child.name == name) {
                    return child.gameObject;
                }
            }
            return null;
        }

        public bool IsSameAsSolutionMaze() {
            return MyMazeGraph.ContainsSubgraph(SolMazeManagerInstance.ActiveSolMazeGraph) &&
                  (MyMazeGraph.FindMazePieceMisAligned(SolMazeManagerInstance.ActiveSolMazeGraph) == null);
        }

        /// <summary>
        /// Returns a connection if there is a matching request from the collided MazeConnector
        /// </summary>
        /// <param name="requestingMazeConnector">see name</param>
        /// <param name="collidedMazeConnector">see name</param>
        /// <returns></returns>
        internal void AddConnectionRequest(MazeConnector requestingMazeConnector, MazeConnector collidedMazeConnector) {
            // add request to dictionary
            if (!connectRequests.ContainsKey(requestingMazeConnector)) {
                connectRequests.Add(requestingMazeConnector, new HashSet<MazeConnector>());
            }
            Assert.IsTrue(!connectRequests[requestingMazeConnector].Contains(collidedMazeConnector));
            connectRequests[requestingMazeConnector].Add(collidedMazeConnector);
        }

        internal void RemoveRequest(MazeConnector requestingMazeConnector, MazeConnector collidedMazeConnector) {
            Assert.IsTrue(connectRequests.ContainsKey(requestingMazeConnector));
            Assert.IsTrue(connectRequests[requestingMazeConnector].Contains(collidedMazeConnector));
            connectRequests[requestingMazeConnector].Remove(collidedMazeConnector);
        }

        internal void SetUpConnection(MazeConnector requestingMazeConnector) {
            Assert.IsTrue(connectRequests.ContainsKey(requestingMazeConnector));
            foreach (var mc in from MazeConnector mc in connectRequests[requestingMazeConnector]    // get all things requesting my connection
                               where connectRequests.ContainsKey(mc) &&                             // select down only to mcs that have a connection request
                                    connectRequests[mc].Contains(requestingMazeConnector) &&        // select down again to an mc that contains myself in its set
                                    mc.MyConnection == null                                         // select down one more to an mc that doesn't have a connection already
                               select mc) {
                new Connection(requestingMazeConnector, mc); // creates a new connection between the two, `Connection.cs` does the connecting
            }
        }

        public void AddPopulatedConnection(Connection connection) {
            Assert.IsTrue(connection.IsPopulated());
            MarkMazeDirty();
            populatedConnections.Add(connection);
        }

        public void ReturnOpenConnectionToPool(Connection connection) {
            Assert.IsTrue(connection.IsFullyOpen());
            MarkMazeDirty();
            populatedConnections.Remove(connection);
        }


        /// <summary>
        /// Lock and unlock should only be called by `SwitchModeButton.cs` because it is the holder of the state
        /// </summary>
        public void LockMaze() {
            SnapPiecesTogether();
        }
        /// <summary>
        /// Lock and unlock should only be called by `SwitchModeButton.cs` because it is the holder of the state
        /// </summary>
        public void UnlockMaze() {
            ReleasePieces();
        }

        public void MarkMazeDirty() {
            MyMazeGraph.MarkDirty();
        }

        public MazePiece GetClosestKuriMazePiece() {
            return MyMazeGraph.GetClosestKuriMazePiece(BabyKuriManagerInstance.BKTransformManager.KuriPos);
        }

        public MazeConnector GetMazeConnectorRelBKInDir(CodeBlockEnums.Move direction) {
            MazePiece kuriMP = GetClosestKuriMazePiece();
            Assert.IsNotNull(kuriMP);
            Vector3 dir = direction == CodeBlockEnums.Move.Forward ? BabyKuriManagerInstance.BKTransformManager.Forward : BabyKuriManagerInstance.BKTransformManager.Backward;
            Vector3 kuriDir = kuriMP.transform.InverseTransformDirection(dir);
            return kuriMP.GetConnector(kuriDir);
        }

        public MazePiece GetPotentialNextMP(CodeBlockEnums.Move direction) {
            return GetMazeConnectorRelBKInDir(direction)?.ConnectedMP;
        }


        public bool IsBKAtTheGoalNow() {
            return BKCurrentMP == GoalMazePiece;
        }

        public bool ExerciseInFullyCompleteState {
            get {
                return IsBKAtTheGoalNow() && IsSameAsSolutionMaze() && Interpreter.instance.CodeIsFinished();
            }
        }

        public MazePiece GetMissingPiecesFromMaze() {
            Dictionary<MPType, int> solMPs = SolMazeManagerInstance.ActiveSolMazeGraph.GetConnectedMazePiecesCount();
            Dictionary<MPType, int> mazeMPs = MyMazeGraph.GetConnectedMazePiecesCount();
            foreach (KeyValuePair<MPType, int> kvp in solMPs) {
                if (!mazeMPs.Keys.Contains(kvp.Key) || kvp.Value > mazeMPs[kvp.Key]) {
                    return FindClosestPieceOfType(kvp.Key);
                }
            }
            return null;
        }

        public MazePiece GetMisalignedPiece() {
            // this assumes that GetMissingPiecesFromMaze returns null meaning all pieces are technically in the maze but oriented incorrectly
            // not calling an assertion here as to not waste time although not sure if assertions run in the main build or not...
            return MyMazeGraph.FindMazePieceMisAligned(SolMazeManagerInstance.ActiveSolMazeGraph);
        }

        public void LogMaze() {
            // log maze a single time at end of frame using coroutine
            MLM.LogMaze();

        }
        #endregion

        #region private
        // Note this gives back a piece even if it has never been tracked
        private MazePiece FindClosestPieceOfType(MPType type) {
            HashSet<MazePiece> connectedPieces = MyMazeGraph.GetAllConnectedMazePieces();
            MazePiece closest = null;
            float closestDist = float.MaxValue;
            foreach (MazePiece mp in AllMazePieces.Except(connectedPieces)) {
                if (mp.MyMPType == type) {
                    float dist = Vector3.Distance(mp.transform.position, BabyKuriManagerInstance.BKTransformManager.KuriPos);
                    if (dist < closestDist) {
                        closest = mp;
                        closestDist = dist;
                    }
                }
            }
            return closest;
        }

        private void MoveOutOfView() {
            transform.position = new Vector3(0, 100, 0);
        }
        private void AddManipulationHandlersForUnityEditor() {
            foreach (Transform child in transform) {
                if (child.GetComponent<ManipulationHandler>()) {
                    return; // already added on last enable
                }
                ManipulationHandler manipHandler = child.gameObject.AddComponent<ManipulationHandler>();
                manipHandler.ManipulationType = ManipulationHandler.HandMovementType.OneHandedOnly;
            }
        }

        private void ReleasePieces() {
            foreach (Transform child in transform) {
                MazePiece mazePiece = child.GetComponent<MazePiece>();
                if (mazePiece != null) {
                    mazePiece.UnAnchor();
                }
            }
            SolMazeManagerInstance.ReleasePieces();
            OnMazeUnlocked.Invoke();
            LoggingManagerInstance.UpdateLogColumn(mazeLockCol, "Unlocked");
        }

        private void SnapPiecesTogether() {
            BKMazePiece.SnapConnections();
            BKTransformManager?.SetOriginalState();
            SolMazeManagerInstance?.SnapPiecesTogether();
            // move all pieces that aren't in my graph way away
#if !UNITY_EDITOR
            DeactivateUnusedMazePieces();
#endif
            OnMazeLocked.Invoke();
            LoggingManagerInstance.UpdateLogColumn(mazeLockCol, "Locked");
        }

        private void DeactivateUnusedMazePieces() {
            HashSet<MazePiece> connectedPieces = MyMazeGraph.GetAllConnectedMazePieces();
            foreach (Transform child in transform) {
                MazePiece mazePiece = child.GetComponent<MazePiece>();
                if (mazePiece != null) {
                    if (!connectedPieces.Contains(mazePiece)) {
                        mazePiece.transform.position = new Vector3(0, 100, 0);
                    }
                }
            }
        }

        internal void UpdateCurrentMP(MazePiece potentialNextPiece) {
            BKCurrentMP = potentialNextPiece;
        }
        #endregion
    }
}
