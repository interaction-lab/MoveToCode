using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

namespace MoveToCode {
    public class MazeManager : Singleton<MazeManager> {
        #region members
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
        MazePiece BKMazePiece {
            get {
                if (bkMazePiece == null) {
                    bkMazePiece = BabyKuriManagerInstance.transform.parent.GetComponent<MazePiece>();
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
        #endregion

        #region unity
        private void OnEnable() {
            ARTrackingManagerInstance.OnTrackingStarted.AddListener(OnTrackingStarted);
            ARTrackingManagerInstance.OnTrackingEnded.AddListener(OnTrackingEnded);
#if UNITY_EDITOR
            AddManipulationHandlersForUnityEditor();
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

        internal void RemoveConnectionRequest(MazeConnector requestingMazeConnector, MazeConnector collidedMazeConnector) {
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
            populatedConnections.Add(connection);
        }

        public void ReturnOpenConnectionToPool(Connection connection, MazeConnector first, MazeConnector second) {
            Assert.IsTrue(connection.IsOpen());
            populatedConnections.Remove(connection);

        }
        #endregion

        #region private
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
            foreach (Transform child in transform) {
                MazePiece mazePiece = child.GetComponent<MazePiece>();
                if (mazePiece != null) {
                    mazePiece.UnAnchor();
                }
            }
        }

        private void SnapPiecesTogether() {
            BKMazePiece.SnapConnections();
            BKTransformManager.SetOriginalState();
        }
        #endregion
    }
}
