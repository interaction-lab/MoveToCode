namespace MoveToCode {
    public class DivisionInstruction : MathInstruction {
        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            return new InstructionReturnValue(new FloatDataType(leftNum / rightNum), GetNextInstruction());
        }
    }
}