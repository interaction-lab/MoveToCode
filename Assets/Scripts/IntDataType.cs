namespace MoveToCode {
    public class IntDataType : INumberDataType {
        public IntDataType(CodeBlock cbIn) : base(cbIn) { }
        public IntDataType(CodeBlock cbIn, int valIn) : base(cbIn, valIn) { }
        public IntDataType(int valIn) : base(null, valIn) { }
    }
}
