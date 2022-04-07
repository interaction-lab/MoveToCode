using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class Connection : MonoBehaviour {
        #region members
        MazeManager mazeManager;
        MazeManager MazeManagerInstance {
            get {
                if (mazeManager == null) {
                    mazeManager = MazeManager.instance;
                }
                return mazeManager;
            }
        }
        Pair<MazeConnector, MazeConnector> mazeConnectors = new Pair<MazeConnector, MazeConnector>(null, null);
        bool oneMemberHasRequestedADisconnect = false;

        public Color connectedColor;

        #endregion

        #region unity
        #endregion

        #region public
        public bool IsPopulated() {
            return mazeConnectors.First != null && mazeConnectors.Second != null;
        }

        public bool IsOpen() {
            return !IsPopulated();
        }

        public void RequestDisconnect() {
            if (oneMemberHasRequestedADisconnect) {
                BreakConnection();
                oneMemberHasRequestedADisconnect = false;
            }
            else {
                oneMemberHasRequestedADisconnect = true;
            }
        }

        internal void BreakConnection() {
            mazeConnectors.First.Disconnect();
            mazeConnectors.Second.Disconnect();
            mazeConnectors.First = null;
            mazeConnectors.Second = null;

            MazeManagerInstance.ReturnOpenConnectionToPool(this);
        }

        internal void Connect(MazeConnector mazeConnector, MazeConnector otherMazeConnector) {
            mazeConnectors.First = mazeConnector;
            mazeConnectors.Second = otherMazeConnector;
            otherMazeConnector.MyConnection = this;

            mazeConnector.MyMazePiece.transform.parent = transform;
            otherMazeConnector.MyMazePiece.transform.parent = transform;

            connectedColor = UnityEngine.Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f, 1f, 1f);
            mazeConnector.SetColor(connectedColor);
            otherMazeConnector.SetColor(connectedColor);

            MazeManagerInstance.AddPopulatedConnection(this);
        }
        #endregion

        #region private
        #endregion
    }
}
