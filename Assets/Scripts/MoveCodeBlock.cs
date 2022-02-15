namespace MoveToCode {
    public class MoveCodeBlock : StandAloneInstructionCodeBlock {
        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new MoveInstruction(this);
        }
    }
}