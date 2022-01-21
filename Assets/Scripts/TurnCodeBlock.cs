namespace MoveToCode {
    public class TurnCodeBlock : StandAloneInstructionCodeBlock {
        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new TurnInstruction(this);
        }
    }
}