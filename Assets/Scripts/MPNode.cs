using static MoveToCode.MazePiece;

namespace MoveToCode {
    public readonly struct MPNode {
        public readonly MazePiece MyPiece;
        public readonly MPType MyType;
        public readonly CONNECTDIR MyDir;

        public MPNode(MazePiece piece, CONNECTDIR dir) {
            MyPiece = piece;
            MyDir = dir;
            MyType = MyPiece.MyMPType;
        }

        //json output of mpnode
        public override string ToString() {
            return "{" +
            "\"MP\": " + MyPiece.GetHashCode() +
            ", \"D\": \"" + MyDir +
            "\", \"MT\": " + MyType.ToString() +
            "}";
        }

        public bool Equals(MPNode other) {
            return MyType.Equals(other.MyType) && SameDirClass(other.MyDir);
        }

        // Deals with symtrical directions
        public bool SameDirClass(CONNECTDIR dir) {
            return MyDir == dir ||
            (MyDir == CONNECTDIR.North && dir == CONNECTDIR.South) ||
            (MyDir == CONNECTDIR.South && dir == CONNECTDIR.North) ||
            (MyDir == CONNECTDIR.East && dir == CONNECTDIR.West) ||
            (MyDir == CONNECTDIR.West && dir == CONNECTDIR.East);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

    }
}
