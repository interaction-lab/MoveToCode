namespace MoveToCode {
    public abstract class StandAloneInstructionCodeBlock : InstructionCodeBlock {
        public StandAloneInstruction GetNextInstruction() {
            return (myBlockInternalArg as StandAloneInstruction).GetNextInstruction();
        }
    }
}