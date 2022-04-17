using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MoveToCode.MazePiece;

namespace MoveToCode {
    public class MazeGraph {
        #region members
        public MazePiece Root {
            get;
            private set;
        }
        HashSet<MPEdge> edges;
        HashSet<MazePiece> visitedPieces;
        #endregion

        #region public
        public MazeGraph(MazePiece _root) {
            Root = _root;
        }

        public HashSet<MPEdge> GetAllEdges() {
            ResetGraph();
            PopulateEdges(Root);
            return edges;
        }

        public bool ContainsSubgraph(MazeGraph other) {
            GetAllEdges();
            foreach (MPEdge otherEdge in other.GetAllEdges()) {
                bool found = false;
                foreach (MPEdge myEdge in edges) {
                    if (myEdge.Equals(otherEdge)) {
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    return false;
                }
            }
            return true;
        }

        public override string ToString() {
            return '[' + string.Join(',', GetAllEdges()) + ']';
        }
        #endregion

        #region private
        private void ResetGraph() {
            edges = new HashSet<MPEdge>();
            visitedPieces = new HashSet<MazePiece>();
        }
        private void PopulateEdges(MazePiece current) {
            if (visitedPieces.Contains(current)) {
                return;
            }
            visitedPieces.Add(current);
            foreach (CONNECTDIR dir in current.ConnectionDict.Keys) {
                MazePiece nextPiece = current.ConnectionDict[dir].ConnectedMP;
                if (nextPiece != null) {
                    edges.Add(current.ConnectionDict[dir].MyConnection.GetMPEdge());
                    PopulateEdges(nextPiece);
                }
            }
        }
        #endregion
    }
}
