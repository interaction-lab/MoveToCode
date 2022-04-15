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
        HashSet<MazePiece> visitedPieces;
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
            if (visitedPieces.Contains(current)) {
                return allPieces;
            }
            visitedPieces.Add(current);
            foreach (CONNECTDIR dir in current.ConnectionDict.Keys) {
                MazePiece nextPiece = current.ConnectionDict[dir].ConnectedMP;

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
            if (otherPiece == null) {
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
