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

        #endregion

        #region public
        public MPType(bool north, bool south, bool east, bool west, bool babyKuri, bool goal) {
            North = north;
            South = south;
            East = east;
            West = west;
            BabyKuri = babyKuri;
            Goal = goal;
        }

        public MPType(Dictionary<CONNECTDIR, MazeConnector> connectionsDict, bool babyKuri, bool goal) {
            North = connectionsDict.ContainsKey(CONNECTDIR.North) && connectionsDict[CONNECTDIR.North] != null;
            South = connectionsDict.ContainsKey(CONNECTDIR.South) && connectionsDict[CONNECTDIR.South] != null;
            East = connectionsDict.ContainsKey(CONNECTDIR.East) && connectionsDict[CONNECTDIR.East] != null;
            West = connectionsDict.ContainsKey(CONNECTDIR.West) && connectionsDict[CONNECTDIR.West] != null;
            BabyKuri = babyKuri;
            Goal = goal;
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

        public override string ToString() {
            return "North(" + North + ")_South(" + South + ")_East(" + East + ")_West(" + West + ")_BabyKuri(" + BabyKuri + ")_Goal(" + Goal + ")";
        }

        public bool IsNull() {
            return !(North || South || East || West || BabyKuri || Goal);
        }
        #endregion

        #region private
        #endregion
    }
}