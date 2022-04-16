using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MoveToCode.MazePiece;

namespace MoveToCode {
    public readonly struct MPType : IEquatable<MPType> {
        #region members
        public bool North { get; }
        public bool South { get; }
        public bool East { get; }
        public bool West { get; }
        public bool BabyKuri { get; }
        public bool Goal { get; }

        private static long _id = 0;
        public static long IDCoutnter {
            get {
                _id++;
                return _id;
            }
        }
        private readonly long MyID { get; }

        #endregion

        #region public
        public MPType(bool north, bool south, bool east, bool west, bool babyKuri, bool goal) {
            North = north;
            South = south;
            East = east;
            West = west;
            BabyKuri = babyKuri;
            Goal = goal;
            MyID = IDCoutnter;
        }

        public MPType(Dictionary<CONNECTDIR, MazeConnector> connectionsDict, bool babyKuri, bool goal) {
            North = connectionsDict.ContainsKey(CONNECTDIR.North) && connectionsDict[CONNECTDIR.North] != null;
            South = connectionsDict.ContainsKey(CONNECTDIR.South) && connectionsDict[CONNECTDIR.South] != null;
            East = connectionsDict.ContainsKey(CONNECTDIR.East) && connectionsDict[CONNECTDIR.East] != null;
            West = connectionsDict.ContainsKey(CONNECTDIR.West) && connectionsDict[CONNECTDIR.West] != null;
            BabyKuri = babyKuri;
            Goal = goal;
            MyID = IDCoutnter;
        }

        public override bool Equals(object obj) {
            return obj is MPType type && Equals(type);
        }

        public bool Equals(MPType other) {
            return North == other.North &&
                   South == other.South &&
                   East == other.East &&
                   West == other.West &&
                   BabyKuri == other.BabyKuri &&
                   Goal == other.Goal;
        }

        public override int GetHashCode() {
            return HashCode.Combine(North, South, East, West, BabyKuri, Goal);
        }

        public static bool operator ==(MPType left, MPType right) {
            return left.Equals(right);
        }

        public static bool operator !=(MPType left, MPType right) {
            return !(left == right);
        }

        // json output of mptype
        public override string ToString() {
            return "{" + $"'ID': {MyID}, 'N': {North}, 'S': {South}, 'E': {East}, 'W': {West}, 'B': {BabyKuri}, 'G': {Goal}" + "}";
        }


        public bool IsNull() {
            return !(North || South || East || West || BabyKuri || Goal);
        }
        #endregion

        #region private
        #endregion
    }
}
