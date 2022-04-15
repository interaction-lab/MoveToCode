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
            "'MP': " + MyPiece +
            ", 'D': " + MyDir +
            ", 'MT': " + MyType.ToString() +
            "}";
        }

    }
}
