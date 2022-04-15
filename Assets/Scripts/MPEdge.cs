namespace MoveToCode {
    public readonly struct MPEdge {
        public readonly Pair<MPNode, MPNode> Nodes;

        public MPEdge(MPNode node1, MPNode node2) {
            Nodes = new Pair<MPNode, MPNode>(node1, node2);
        }

        public override string ToString() {
            return "{" + "'N1': " + Nodes.First.ToString() + ", 'N2': " + Nodes.Second.ToString() + "}";
        }

        public override bool Equals(object obj) {
            if (obj is MPEdge) {
                MPEdge other = (MPEdge)obj;
                return (Nodes.First.Equals(other.Nodes.First) && Nodes.Second.Equals(other.Nodes.Second))
                || (Nodes.First.Equals(other.Nodes.Second) && Nodes.Second.Equals(other.Nodes.First));
            }
            return false;
        }
    }
}
