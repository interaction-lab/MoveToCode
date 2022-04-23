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

        public static Dictionary<CONNECTDIR, Vector3> dirToVector = new Dictionary<CONNECTDIR, Vector3> {
            {CONNECTDIR.North, new Vector3(0, 0, 1)},
            {CONNECTDIR.South, new Vector3(0, 0, -1)},
            {CONNECTDIR.East, new Vector3(1, 0, 0)},
            {CONNECTDIR.West, new Vector3(-1, 0, 0)}
        };

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
        public bool IsAnchored {
            get; set;
        }
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
        Color _color = Color.magenta;
        public Color MyColor {
            get {
                if (_color == Color.magenta) {
                    _color = GetComponent<Renderer>().material.color;
                }
                return _color;
            }
        }
        #endregion

        #region unity
        void Awake() {
            IsAnchored = false;
            // add TrashButtonCanvas using resource loading
            if (GetComponentInChildren<TrashButton>() == null) {
                Instantiate(Resources.Load<GameObject>(ResourcePathConstants.TrashButtonCanvasPrefab), transform);
            }
        }
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
                kvp.Value?.TurnOffConnector();
            }
        }

        internal void UnAnchor() {
            IsAnchored = false;
            foreach (MazeConnector mazeConnector in ConnectionDict.Values) {
                mazeConnector.TurnOnConnector();
            }
        }

        public MazeConnector GetConnector(Vector3 direction) {
            Vector3 roundedDir = new Vector3(Mathf.Round(direction.x), 0, Mathf.Round(direction.z));
            foreach (KeyValuePair<CONNECTDIR, MazeConnector> kvp in ConnectionDict) {
                if (dirToVector[kvp.Key] == roundedDir) {
                    return kvp.Value;
                }
            }
            return null;
        }

        public void DisableMyPiece() {
            // move my piece way off screen so that all systems work as if I removed the peice to another location
            transform.position = new Vector3(0, -100, 0);
        }

        #endregion

        #region protected
        protected virtual void SetUpOnEnable() {
            arTrackBehavior.OnImgStartedTracking.AddListener(OnImgStartedTracking);
            arTrackBehavior.OnImgStoppedTracking.AddListener(OnImgStoppedTracking);
            SetUpLayerMask(LayerMask.NameToLayer(LayerMaskConstants.MAZEPIECE));
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

        protected void SetUpLayerMask(LayerMask lm) {
            gameObject.layer = lm;
        }

        public override string ToString() {
            return MyMPType.ToString();
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
