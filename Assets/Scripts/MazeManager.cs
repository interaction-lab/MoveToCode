using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MoveToCode {
    public class MazeManager : Singleton<MazeManager> {
        #region members
        Queue<Connection> openConnections = new Queue<Connection>();
        HashSet<Connection> populatedConnections = new HashSet<Connection>();
        HashSet<Pair<MazeConnector, MazeConnector>> connectRequests = new HashSet<Pair<MazeConnector, MazeConnector>>();
        MazePiece bkMazePiece;
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
        internal Connection RequestConnection(MazeConnector requestingMazeConnector, MazeConnector collidedMazeConnector) {
            if (CheckForMatchingRequest(requestingMazeConnector, collidedMazeConnector)) {
                return GetOpenConnection();
            }
            return null;
        }

        public void AddPopulatedConnection(Connection connection) {
            populatedConnections.Add(connection);
        }

        public void ReturnOpenConnectionToPool(Connection connection) {
            Assert.IsTrue(connection.IsOpen());
            populatedConnections.Remove(connection);
            openConnections.Enqueue(connection);
        }
        #endregion

        #region private
        private bool CheckForMatchingRequest(MazeConnector requestingMazeConnector, MazeConnector collidedMazeConnector) {
            // connection requests holds connections in order of (requester, collided)
            Pair<MazeConnector, MazeConnector> potentialMatchingRequest = new Pair<MazeConnector, MazeConnector>(collidedMazeConnector, requestingMazeConnector);
            bool hasMatchingRequest = connectRequests.Contains(potentialMatchingRequest);
            if (hasMatchingRequest) {
                connectRequests.Remove(potentialMatchingRequest);
                return true;
            }
            connectRequests.Add(new Pair<MazeConnector, MazeConnector>(requestingMazeConnector, collidedMazeConnector));
            return false;
        }
        private Connection GetOpenConnection() {
            if (!openConnections.Empty()) {
                return openConnections.Dequeue();
            }
            GameObject connectionGO = Instantiate(Resources.Load<GameObject>(ResourcePathConstants.ConnectionP));
            connectionGO.transform.SetParent(transform);
            return connectionGO.GetComponent<Connection>();
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
            foreach (Transform child in transform) {
                MazePiece mazePiece = child.GetComponent<MazePiece>();
                if (mazePiece != null) {
                    mazePiece.UnAnchor();
                }
            }
        }

        private void SnapPiecesTogether() {
            // tell kuri maze piece to recruse through rest of peices
            // this will be a problem at some point because I am too lazy to make a new maze piece specifically for baby kuri
            BabyKuriManager.instance.transform.parent.GetComponent<MazePiece>().SnapConnections();
        }
        #endregion
    }
}
