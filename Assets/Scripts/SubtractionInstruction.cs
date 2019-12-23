namespace MoveToCode {
    public class SubtractionInstruction : MathInstruction {
        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            return new InstructionReturnValue(new FloatDataType(leftNum - rightNum), GetNextInstruction());
        }

        public override string GetMathSymbol() {
            return "-";
        }
    }
}