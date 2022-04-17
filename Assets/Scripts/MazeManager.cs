using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;
using UnityEngine.Events;

namespace MoveToCode {
    public class MazeManager : Singleton<MazeManager> {
        #region members
        public UnityEvent OnMazeLocked, OnMazeUnlocked;
        public static string mazeLogCol = "UserMaze";
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
                        if (t.GetComponent<MazeBabyKuri>() != null) {
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
        bool IsLocked = false;
        bool hasBeenInitialized = false;
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
        private void OnEnable() {
            ARTrackingManagerInstance.OnTrackingStarted.AddListener(OnTrackingStarted);
            ARTrackingManagerInstance.OnTrackingEnded.AddListener(OnTrackingEnded);
            if (!hasBeenInitialized) {
                LoggingManagerInstance.AddLogColumn(mazeLogCol, "");
                hasBeenInitialized = true;
            }
#if UNITY_EDITOR
            AddManipulationHandlersForUnityEditor();
#else
            MoveOutOfView(); // Move the maze out of view so that it doesn't get in the way of the user when deployed to a device
#endif
        }
        private void OnDisable() {
            ARTrackingManagerInstance.OnTrackingStarted.RemoveListener(OnTrackingStarted);
            ARTrackingManagerInstance.OnTrackingEnded.RemoveListener(OnTrackingEnded);
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
            populatedConnections.Add(connection);
        }

        public void ReturnOpenConnectionToPool(Connection connection) {
            Assert.IsTrue(connection.IsFullyOpen());
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
        #endregion

        #region private
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

        private void OnTrackingStarted() {
            ReleasePieces();
        }
        private void OnTrackingEnded() {
            // where we snap the maze to each other + floor + grid
            SnapPiecesTogether();
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
        }

        private void SnapPiecesTogether() {
            if (IsLocked) {
                return;
            }
            BKMazePiece.SnapConnections();
            BKTransformManager.SetOriginalState();
            SolMazeManagerInstance.SnapPiecesTogether();
            IsLocked = true;
            OnMazeLocked.Invoke();
            LoggingManagerInstance.UpdateLogColumn(mazeLogCol, MyMazeGraph.ToString());
        }
        #endregion
    }
}
