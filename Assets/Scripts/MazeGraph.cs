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
        HashSet<Pair<MazePiece, CONNECTDIR>> visited;
        #endregion

        #region public
        public MazeGraph(MazePiece _root) {
            Root = _root;
        }

        public bool HasSubGraph(MazeGraph other) {
            ResetVisited();
            return TraverseForSubGraphDiscrepency(Root, other.Root, CONNECTDIR.North);

        }

        public List<MPEdge> GetAllPieces() {
            List<MPEdge> allPieces = new List<MPEdge>();
            // traverse through whole maze and collect MPEdges
            return TraverseForAllPieces(Root, allPieces);
        }

        #endregion

        #region private

        private List<MPEdge> TraverseForAllPieces(MazePiece current, List<MPEdge> allPieces) {
            if (visited.Contains(new Pair<MazePiece, CONNECTDIR>(current, CONNECTDIR.North))) {
                return allPieces;
            }
            visited.Add(new Pair<MazePiece, CONNECTDIR>(current, CONNECTDIR.North));
            if (current.MyMPType.North) {
                allPieces.Add(new MPEdge(new MPNode(current, CONNECTDIR.North), new MPNode(current.North, CONNECTDIR.South)));
            }
            if (current.MyMPType.South) {
                allPieces.Add(new MPEdge(new MPNode(current, CONNECTDIR.South), new MPNode(current.South, CONNECTDIR.North)));
            }
            if (current.MyMPType.East) {
                allPieces.Add(new MPEdge(new MPNode(current, CONNECTDIR.East), new MPNode(current.East, CONNECTDIR.West)));
            }
            if (current.MyMPType.West) {
                allPieces.Add(new MPEdge(new MPNode(current, CONNECTDIR.West), new MPNode(current.West, CONNECTDIR.East)));
            }
            if (current.MyMPType.North) {
                TraverseForAllPieces(current.North, allPieces);
            }
            if (current.MyMPType.South) {
                TraverseForAllPieces(current.South, allPieces);
            }
            if (current.MyMPType.East) {
                TraverseForAllPieces(current.East, allPieces);
            }
            if (current.MyMPType.West) {
                TraverseForAllPieces(current.West, allPieces);
            }
            return allPieces;
        }

        private bool SamePieceType(MazePiece a, MazePiece b) {
            return a?.MyMPType == b?.MyMPType;
        }

        private bool TraverseForSubGraphDiscrepency(MazePiece myPiece, MazePiece otherPiece, CONNECTDIR dir) {
            if (!SamePieceType(myPiece, otherPiece)) { // make sure to compare types of maze pieces as opposed to pointers
                return false;
            }
            if(otherPiece == null){
                return true;
            }
            visited.Add(new Pair<MazePiece, CONNECTDIR>(myPiece, dir));
            foreach (CONNECTDIR direction in otherPiece.ConnectionDict.Keys) {
                MazePiece otherNextPiece = otherPiece.ConnectionDict[direction].ConnectedMP;
                if (otherNextPiece == null) {
                    continue;
                }

                if (direction == CONNECTDIR.North || direction == CONNECTDIR.South) {
                    // check north and south
                    MazePiece myNorthPiece = myPiece.ConnectionDict[CONNECTDIR.North].ConnectedMP;
                    bool northTraversal = TraverseForSubGraphDiscrepency(myNorthPiece, otherNextPiece, CONNECTDIR.North);
                    MazePiece mySouthPiece = myPiece.ConnectionDict[CONNECTDIR.South].ConnectedMP;
                    bool southTraversal = TraverseForSubGraphDiscrepency(mySouthPiece, otherNextPiece, CONNECTDIR.South);
                    return northTraversal || southTraversal;

                }
                else {
                    // check east and west
                    MazePiece myEastPiece = myPiece.ConnectionDict[CONNECTDIR.East].ConnectedMP;
                    bool eastTraversal = TraverseForSubGraphDiscrepency(myEastPiece, otherNextPiece, CONNECTDIR.East);
                    MazePiece myWestPiece = myPiece.ConnectionDict[CONNECTDIR.West].ConnectedMP;
                    bool westTraversal = TraverseForSubGraphDiscrepency(myWestPiece, otherNextPiece, CONNECTDIR.West);
                    return eastTraversal || westTraversal;
                }
            }
            return true;
        }

        private void ResetVisited() {
            visited = new HashSet<Pair<MazePiece, CONNECTDIR>>();
            // pretend visited all of the frontier of a graph
            visited.Add(new Pair<MazePiece, CONNECTDIR>(null, CONNECTDIR.North));
            visited.Add(new Pair<MazePiece, CONNECTDIR>(null, CONNECTDIR.South));
            visited.Add(new Pair<MazePiece, CONNECTDIR>(null, CONNECTDIR.East));
            visited.Add(new Pair<MazePiece, CONNECTDIR>(null, CONNECTDIR.West));
            // pretend visited root from all directions except north (north starts the traversal)]
            visited.Add(new Pair<MazePiece, CONNECTDIR>(Root, CONNECTDIR.South));
            visited.Add(new Pair<MazePiece, CONNECTDIR>(Root, CONNECTDIR.East));
            visited.Add(new Pair<MazePiece, CONNECTDIR>(Root, CONNECTDIR.West));
        }
        #endregion
    }
}
