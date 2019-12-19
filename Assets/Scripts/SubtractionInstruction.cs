namespace MoveToCode {
    public class SubtractionInstruction : MathInstruction {
        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            return new InstructionReturnValue(new IntDataType(leftInt - rightInt), GetNextInstruction());
        }
    }
}