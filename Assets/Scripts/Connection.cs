using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace MoveToCode {
    public class Connection {
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
        public bool OneMemberHasRequestedDisconnect = false;
        public Color connectedColor;

        public bool BothPiecesAreAnchored {
            get {
                return IsPopulated() && mazeConnectors.First.IsAnchored && mazeConnectors.Second.IsAnchored;
            }
        }

        #endregion

        #region public
        public Connection(MazeConnector first, MazeConnector second) {
            Connect(first, second);
        }

        public MPEdge GetMPEdge() {
            MPNode node1 = new MPNode(mazeConnectors.First.MyMazePiece, mazeConnectors.First.connectionDir);
            MPNode node2 = new MPNode(mazeConnectors.Second.MyMazePiece, mazeConnectors.Second.connectionDir);
            return new MPEdge(node1, node2);
        }

        internal MazePiece GetConnectedPiece(MazeConnector requestingConnector) {
            Assert.IsTrue(IsPopulated());
            if (requestingConnector == mazeConnectors.First) {
                return mazeConnectors.Second.MyMazePiece;
            }
            return mazeConnectors.First.MyMazePiece;
        }

        public bool IsPopulated() {
            return mazeConnectors.First != null && mazeConnectors.Second != null;
        }

        public bool IsPartiallyOpen() {
            return !IsPopulated() && (mazeConnectors.First != null || mazeConnectors.Second != null);
        }

        public bool IsFullyOpen() {
            return !IsPopulated() && !IsPartiallyOpen();
        }

        internal void AnchorNeighboringPiece() {
            Assert.IsTrue(IsPopulated());
            // find if pieces are anchored already
            if (mazeConnectors.First.IsAnchored && mazeConnectors.Second.IsAnchored) {
                return;
            }
            // find which piece is already anchored
            MazeConnector anchorPiece = null;
            MazeConnector nonAnchorPiece = null;
            if (mazeConnectors.First.IsAnchored) {
                anchorPiece = mazeConnectors.First;
                nonAnchorPiece = mazeConnectors.Second;
            }
            else if (mazeConnectors.Second.IsAnchored) {
                anchorPiece = mazeConnectors.Second;
                nonAnchorPiece = mazeConnectors.First;
            }
            // anchor the non-anchor piece to the anchor piece
            nonAnchorPiece.AnchorTo(anchorPiece);
        }
        #endregion

        #region private
        private void Connect(MazeConnector mazeConnector, MazeConnector otherMazeConnector) {
            mazeConnectors.First = mazeConnector;
            mazeConnectors.Second = otherMazeConnector;
            mazeConnector.MyConnection = this;
            otherMazeConnector.MyConnection = this;

            connectedColor = Color.Lerp(mazeConnector.MyMazePiece.MyColor, otherMazeConnector.MyMazePiece.MyColor, 0.5f);
            connectedColor.a = 1f;
            mazeConnector.SetColor(connectedColor);
            otherMazeConnector.SetColor(connectedColor);

            MazeManagerInstance.AddPopulatedConnection(this);
        }

        internal void RequestDisconnect() {
            if (OneMemberHasRequestedDisconnect) {
                mazeConnectors.First?.Disconnect();
                mazeConnectors.Second?.Disconnect();
                mazeConnectors.First = null;
                mazeConnectors.Second = null;
                MazeManagerInstance.ReturnOpenConnectionToPool(this);
            }
            else {
                OneMemberHasRequestedDisconnect = true;
            }
        }
        #endregion
    }
}
