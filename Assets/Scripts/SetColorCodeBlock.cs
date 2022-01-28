namespace MoveToCode {
    public class SetColorCodeBlock : StandAloneInstructionCodeBlock {
        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new SetColorInstruction(this);
        }
    }
}