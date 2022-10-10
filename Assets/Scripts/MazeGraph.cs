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
            isDirty = true; // temporary caching fix TODO: make this actually work
            if (isDirty) {
                ResetGraph();
                PopulateEdges(Root);
                isDirty = false;
            }
            return edges;
        }

        public Dictionary<MPType, int> GetConnectedMazePiecesCount() {
            GetAllEdges();
            Dictionary<MPType, int> connectedPieces = new Dictionary<MPType, int>();
            foreach (MazePiece mp in mazePieces) {
                if (connectedPieces.ContainsKey(mp.MyMPType)) {
                    connectedPieces[mp.MyMPType]++;
                }
                else {
                    connectedPieces.Add(mp.MyMPType, 1);
                }
            }
            return connectedPieces;
        }

        public HashSet<MazePiece> GetAllConnectedMazePieces() {
            GetAllEdges();
            return mazePieces;
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

        public MazePiece FindMazePieceMisAligned(MazeGraph other) { // other is solution
            // look through my connected pieces
            foreach (MazePiece myMazePiece in GetAllConnectedMazePieces()) {
                // look through other connected pieces
                foreach (MazePiece otherMazePiece in other.GetAllConnectedMazePieces()) {
                    // check out types are the same
                    if (myMazePiece.MyMPType != otherMazePiece.MyMPType) {
                        continue;
                    }
                    // if connections of my maze
                    foreach (MazePiece.CONNECTDIR dir in MazePiece.dirToVector.Keys) {
                        if (!myMazePiece.ConnectionDict.ContainsKey(dir)) {
                            continue;
                        }

                        if (DifferentConnection(myMazePiece, otherMazePiece, dir) > 0) {
                            return myMazePiece;
                        }
                    }

                }
            }
            return null;
        }

        // 0 if the same
        // -1 if mp0 is null and mp1 is not
        // 1 if mp0 is not null and mp1 is null 
        // 2 if mp0 is just different than mp1
        private int DifferentConnection(MazePiece mp0, MazePiece mp1, MazePiece.CONNECTDIR dir) {
            MazePiece mp0Neighbor = mp0.ConnectionDict[dir].ConnectedMP;
            MazePiece mp1Neighbor = mp1.ConnectionDict[dir].ConnectedMP;
            if (mp0Neighbor == null && mp1Neighbor == null) {
                return 0;
            }
            else if (mp0Neighbor == null && mp1Neighbor != null) {
                return -1;
            }

            else if (mp0Neighbor != null && mp1Neighbor == null) {
                return 1;
            }
            else if (mp0Neighbor.MyMPType != mp1Neighbor.MyMPType) {
                return 2;
            }
            return 0; // they are not null and of the same type
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
