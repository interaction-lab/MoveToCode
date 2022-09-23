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
        ARTrackingManager aRTrackingManager;
        ARTrackingManager ARTrackingManagerInstance {
            get {
                if (aRTrackingManager == null) {
                    aRTrackingManager = ARTrackingManager.instance;
                }
                return aRTrackingManager;
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
        public bool IsLocked = false;
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

        public bool BKAtGoal = false;
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
        #endregion
        #region unity
        private void OnEnable() {
            if (!hasBeenInitialized) {
                LoggingManagerInstance.AddLogColumn(mazeLogCol, "");
                LoggingManagerInstance.AddLogColumn(mazeLockCol, "");
                LoggingManagerInstance.AddLogColumn(containsSolCol, "");
                hasBeenInitialized = true;
                OnBKGoalEnter = new UnityEvent();
                OnBKGoalExit = new UnityEvent();
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

        public bool ContainsSolutionMaze() {
            return MyMazeGraph.ContainsSubgraph(SolMazeManagerInstance.ActiveSolMazeGraph);
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

        internal Connection GetConnection(MazeConnector requestingMazeConnector) {
            Assert.IsTrue(connectRequests.ContainsKey(requestingMazeConnector));
            foreach (var mc in from MazeConnector mc in connectRequests[requestingMazeConnector]// check for matching request
                               where connectRequests.ContainsKey(mc) && connectRequests[mc].Contains(requestingMazeConnector) && mc.MyConnection == null
                               select mc) {
                // check for matching connection
                return new Connection(requestingMazeConnector, mc);
            }
            return null;
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

        public void ToggleMazeLock() {
            if (IsLocked) {
                ReleasePieces();
            }
            else {
                SnapPiecesTogether();
            }
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

        public void BKIsMovingToPiece(MazePiece mp) {
            UpdateBKGoalEvents(mp);
        }

        public bool IsBKAtTheGoalNow() {
            return BKAtGoal || GetClosestKuriMazePiece()?.GetComponent<MazeGoal>() != null;
        }

        public MazePiece GetMisalignedPiece() {
            Dictionary<MPType, int> solMPs = SolMazeManagerInstance.ActiveSolMazeGraph.GetConnectedMazePiecesCount();
            Dictionary<MPType, int> mazeMPs = MyMazeGraph.GetConnectedMazePiecesCount();
            foreach (KeyValuePair<MPType, int> kvp in solMPs) {
                if (!mazeMPs.Keys.Contains(kvp.Key) || kvp.Value > mazeMPs[kvp.Key]) {
                    return FindClosestPieceOfType(kvp.Key);
                }
            }
            return null;
        }

        // TODO: fix this so that it is event driven only or at least handles correctly
        private void Update() {
            BKAtGoal = false;
        }

        public void LogMaze() {
            // log maze a single time at end of frame using coroutine
            StartCoroutine(LogMazeCoroutine());
        }
        #endregion

        #region private
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
        bool loggedThisFrame = false;
        IEnumerator LogMazeCoroutine() {
            yield return new WaitForEndOfFrame();
            if (loggedThisFrame) {
                yield break;
            }
            loggedThisFrame = true;
            LoggingManagerInstance.UpdateLogColumn(mazeLogCol, MyMazeGraph.ToString());
            LoggingManagerInstance.UpdateLogColumn(containsSolCol, ContainsSolutionMaze() ? "1" : "0");
            SolMazeCheckMark.instance.ToggleCheckMark(); // this is super hacky
            yield return new WaitForEndOfFrame();
            loggedThisFrame = false;
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
            if (!IsLocked) {
                return;
            }
            foreach (Transform child in transform) {
                MazePiece mazePiece = child.GetComponent<MazePiece>();
                if (mazePiece != null) {
                    mazePiece.UnAnchor();
                }
            }
            SolMazeManagerInstance.ReleasePieces();
            IsLocked = false;
            OnMazeUnlocked.Invoke();
            LoggingManagerInstance.UpdateLogColumn(mazeLockCol, "Unlocked");
        }
        private void UpdateBKGoalEvents(MazePiece mp) {
            bool lastIsAtGoal = BKAtGoal;
            if(mp?.GetComponent<MazeGoal>()) {
                if (!lastIsAtGoal) {
                    OnBKGoalEnter.Invoke();
                    BKAtGoal = true;
                }
            }
            else {
                if (lastIsAtGoal) {
                    OnBKGoalExit.Invoke();
                    BKAtGoal = false;
                }
            }
        }
        private void SnapPiecesTogether() {
            if (IsLocked) {
                return;
            }
            BKMazePiece.SnapConnections();
            BKTransformManager?.SetOriginalState();
            SolMazeManagerInstance?.SnapPiecesTogether();
            IsLocked = true;
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
        #endregion
    }
}
