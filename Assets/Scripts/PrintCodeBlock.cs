namespace MoveToCode {
    public class PrintCodeBlock : InstructionCodeBlock {
        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new PrintInstruction(this);
        }
    }
}