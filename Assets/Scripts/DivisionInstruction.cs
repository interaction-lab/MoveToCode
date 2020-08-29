namespace MoveToCode {
    public class DivisionInstruction : MathInstruction {
        public DivisionInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            return new InstructionReturnValue(new FloatDataType(leftNum / rightNum), null);
        }

        public override string GetMathSymbol() {
            return "/";
        }
    }
}