using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MazeGraph {
        #region members
        public MazePiece Root {
            get;
            private set;
        }
        HashSet<MazePiece> visited;
        #endregion

        #region public
        public MazeGraph(MazePiece _root) {
            Root = _root;
        }

        public bool HasSubGraph(MazeGraph other) {
            ResetVisited();
            return TraverseForSubGraphDiscrepency(Root, other.Root);

        }
        #endregion

        #region private
        private bool TraverseForSubGraphDiscrepency(MazePiece myPiece, MazePiece otherPiece) {
            if (myPiece?.MyMPType != otherPiece?.MyMPType) { // make sure to compare types of maze pieces as opposed to pointers
                return false;
            }
            visited.Add(myPiece);
            foreach (MazePiece.CONNECTDIR direction in otherPiece.ConnectionDict.Keys) {
                MazePiece otherNextPiece = otherPiece.ConnectionDict[direction].ConnectedMP;
                MazePiece myNextPiece = myPiece.ConnectionDict[direction].ConnectedMP;
                if (otherNextPiece == null || visited.Contains(myNextPiece)) {
                    continue;
                }
                if (!TraverseForSubGraphDiscrepency(myNextPiece, otherNextPiece)) {
                    return false;
                }
            }
            return true;
        }

        private void ResetVisited() {
            visited = new HashSet<MazePiece>();
            visited.Add(null); // pretend visited all of the frontier of a graph
        }
        #endregion
    }
}
