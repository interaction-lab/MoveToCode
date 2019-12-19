namespace MoveToCode {
    public class MultiplicationInstruction : MathInstruction {
        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            return new InstructionReturnValue(new FloatDataType(leftNum * rightNum), GetNextInstruction());
        }
    }
}