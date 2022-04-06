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
        Dictionary<CONNECTDIR, MazeConnector> ConnectionDict {
            get {
                if (connectionsDict == null) {
                    connectionsDict = new Dictionary<CONNECTDIR, MazeConnector>();
                }
                return connectionsDict;
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
        #endregion

        #region unity
        private void OnEnable() {
            arTrackBehavior.OnImgStartedTracking.AddListener(OnImgStartedTracking);
            arTrackBehavior.OnImgStoppedTracking.AddListener(OnImgStoppedTracking);
#if UNITY_EDITOR
            if (manipHandler == null) {
                manipHandler = gameObject.AddComponent<ManipulationHandler>();
                manipHandler.ManipulationType = ManipulationHandler.HandMovementType.OneHandedOnly;
                manipHandler.AllowFarManipulation = true;
            }
            StartCoroutine(WaitForEndOfFrame());
#endif
        }
        private void OnDisable() {
            arTrackBehavior.OnImgStartedTracking.RemoveListener(OnImgStartedTracking);
            arTrackBehavior.OnImgStoppedTracking.RemoveListener(OnImgStoppedTracking);
        }
        #endregion

        #region public
        internal void RegisterConnector(CONNECTDIR connectionDir, MazeConnector mazeConnector) {
            ConnectionDict.Add(connectionDir, mazeConnector);
        }
        #endregion

        #region protected
        #endregion

        #region private
        private void OnImgStartedTracking() {
            foreach (MazeConnector mazeConnector in ConnectionDict.Values) {
                mazeConnector.TurnOnConnector();
            }
        }
        private void OnImgStoppedTracking() {
            foreach (MazeConnector mazeConnector in ConnectionDict.Values) {
                mazeConnector.TurnOffConnector(); // unlikely what will actually do
            }
        }

        IEnumerator WaitForEndOfFrame() {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            OnImgStoppedTracking(); // sets the state of the maze piece as if it was not being tracked
#if UNITY_EDITOR
            OnImgStartedTracking();
#endif
        }
        #endregion
    }
}
