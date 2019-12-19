namespace MoveToCode {
    public class AdditionInstruction : MathInstruction {
        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            return new InstructionReturnValue(new FloatDataType(leftNum + rightNum), GetNextInstruction());
        }
    }
}