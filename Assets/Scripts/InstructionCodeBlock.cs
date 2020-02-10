namespace MoveToCode {
    public abstract class InstructionCodeBlock : CodeBlock {
        public StandAloneInstruction GetNextInstruction() {
            return (myBlockInternalArg as StandAloneInstruction).GetNextInstruction();
        }
    }
}