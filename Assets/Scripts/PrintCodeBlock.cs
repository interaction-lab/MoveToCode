namespace MoveToCode {
    public class PrintCodeBlock : StandAloneInstructionCodeBlock {
        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new PrintInstruction(this);
        }
    }
}