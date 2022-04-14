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
        bool oneMemberHasAttemptedDisconnect = false;
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
            otherMazeConnector.MyConnection = this;

            connectedColor = UnityEngine.Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f, 1f, 1f);
            mazeConnector.SetColor(connectedColor);
            otherMazeConnector.SetColor(connectedColor);

            MazeManagerInstance.AddPopulatedConnection(this);
        }

        internal void RequestDisconnect() {
            if (oneMemberHasAttemptedDisconnect) {
                mazeConnectors.First.Disconnect();
                mazeConnectors.Second.Disconnect();
                mazeConnectors.First = mazeConnectors.Second = null;
                MazeManagerInstance.ReturnOpenConnectionToPool(this);
            }
            else {
                oneMemberHasAttemptedDisconnect = true;
            }
        }
        #endregion
    }
}
