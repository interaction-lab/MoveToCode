using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MazePiece : MonoBehaviour {
        #region members
        public enum CONNECTDIR {
            North,
            South,
            East,
            West
        }

        Dictionary<CONNECTDIR, MazeConnector> connectionsDict;
        public Dictionary<CONNECTDIR, MazeConnector> ConnectionDict {
            get {
                if (connectionsDict == null) {
                    connectionsDict = new Dictionary<CONNECTDIR, MazeConnector>();
                }
                return connectionsDict;
            }
        }

        MPType mpType;
        public MPType MyMPType {
            get {
                if (mpType == null || mpType.IsNull()) {
                    mpType = new MPType(ConnectionDict, IsBabyKuriPiece, IsGoalPiece); // depends on connectionDict to be initialized
                }
                return mpType;
            }
        }

        ARTrackBehavior _aRTrackBehavior;
        ARTrackBehavior arTrackBehavior {
            get {
                if (_aRTrackBehavior == null) {
                    _aRTrackBehavior = GetComponent<ARTrackBehavior>();
                }
                return _aRTrackBehavior;
            }
        }


        ManipulationHandler manipHandler;
        public bool IsAnchored { get; set; }
        public bool IsBabyKuriPiece {
            get {
                return GetComponent<MazeBabyKuri>() != null;
            }
        }
        public bool IsGoalPiece {
            get {
                return GetComponent<MazeGoal>() != null;
            }
        }

        public Vector3 Center {
            get {
                return transform.position;
            }
        }

        #endregion

        #region unity
        private void OnEnable() {
            SetUpOnEnable();
        }

        private void OnDisable() {
            RunOnDisable();
        }
        #endregion

        #region public
        internal void RegisterConnector(CONNECTDIR connectionDir, MazeConnector mazeConnector) {
            ConnectionDict.Add(connectionDir, mazeConnector);
        }

        internal void SnapConnections() {
            IsAnchored = true;
            foreach (KeyValuePair<CONNECTDIR, MazeConnector> kvp in ConnectionDict) {
                TriggerAnchorToThisPiece(kvp.Value);
            }
        }

        internal void UnAnchor() {
            IsAnchored = false;
            foreach (MazeConnector mazeConnector in ConnectionDict.Values) {
                mazeConnector.TurnOnConnector();
            }
        }

        #endregion

        #region protected
        protected virtual void SetUpOnEnable() {
            arTrackBehavior.OnImgStartedTracking.AddListener(OnImgStartedTracking);
            arTrackBehavior.OnImgStoppedTracking.AddListener(OnImgStoppedTracking);
            StartCoroutine(WaitForEndOfFrame());
#if UNITY_EDITOR
            // Add manipulation handler in the case where we aren't building to the real world
            if (manipHandler == null) {
                manipHandler = gameObject.AddComponent<ManipulationHandler>();
                manipHandler.ManipulationType = ManipulationHandler.HandMovementType.OneHandedOnly;
                manipHandler.AllowFarManipulation = true;
            }
#endif
        }
        protected virtual void RunOnDisable() {
            arTrackBehavior.OnImgStartedTracking.RemoveListener(OnImgStartedTracking);
            arTrackBehavior.OnImgStoppedTracking.RemoveListener(OnImgStoppedTracking);
        }
        #endregion

        #region private
        private void OnImgStartedTracking() {
            foreach (MazeConnector mazeConnector in ConnectionDict.Values) {
                mazeConnector.TurnOnConnector();
            }
        }
        private void OnImgStoppedTracking() {
        }
        private void TriggerAnchorToThisPiece(MazeConnector mazeConnector) {
            if (!mazeConnector.IsConnected()) {
                return;
            }
            mazeConnector.AnchorNeighboringPiece();
        }

        IEnumerator WaitForEndOfFrame() {
            yield return new WaitForEndOfFrame();
            OnImgStoppedTracking(); // sets the state of the maze piece as if it was not being tracked
#if UNITY_EDITOR
            OnImgStartedTracking(); // pretend tracking if working in the editor
#endif
        }
        #endregion
    }
}
