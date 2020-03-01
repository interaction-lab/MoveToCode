namespace MoveToCode {
    public class SubtractionInstruction : MathInstruction {

        public SubtractionInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            return new InstructionReturnValue(new FloatDataType(leftNum - rightNum), null);
        }

        public override string GetMathSymbol() {
            return "-";
        }
    }
}