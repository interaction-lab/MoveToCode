using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;

namespace MoveToCode {
    public class MazeGraph {
        #region members
        public MazePiece Root {
            get;
            private set;
        }
        // TODO: check for dirty edges so I can cache when needed
        HashSet<MPEdge> edges = null;
        HashSet<MazePiece> mazePieces = null;
        static long mapIDCounter = 0;
        long _mapID = -1;
        long MapID {
            get {
                if (_mapID == -1) {
                    _mapID = mapIDCounter;
                    mapIDCounter++;
                }
                return _mapID;
            }
        }
        bool isDirty;
        #endregion

        #region public
        public MazeGraph(MazePiece _root) {
            Root = _root;
            mazePieces = new HashSet<MazePiece>();
            edges = new HashSet<MPEdge>();
            isDirty = true;
        }

        public HashSet<MPEdge> GetAllEdges() {
            if (isDirty) {
                ResetGraph();
                PopulateEdges(Root);
                isDirty = false;
            }
            return edges;
        }

        public void MarkDirty() {
            isDirty = true;
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
            GetAllEdges();
            return '[' + string.Join(',', edges) + ']';
        }
        public MazePiece GetClosestKuriMazePiece(Vector3 pos) {
            float closestDist = float.MaxValue;
            GetAllEdges(); // populates all maze pieces // Note this is quite "inefficient" but the number of nodes is minimal
            MazePiece closestPiece = null;
            foreach (MazePiece piece in mazePieces) {
                float dist = Vector3.Distance(pos, piece.transform.position);
                if (dist < closestDist) {
                    closestDist = dist;
                    closestPiece = piece;
                }
            }
            return closestPiece;
        }
        #endregion

        #region private
        private void ResetGraph() {
            edges.Clear();
            mazePieces.Clear();
        }
        private void PopulateEdges(MazePiece current) {
            if (mazePieces.Contains(current)) {
                return;
            }
            mazePieces.Add(current);
            foreach (MazePiece.CONNECTDIR dir in current.ConnectionDict.Keys) {
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
