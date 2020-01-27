namespace MoveToCode {
    public class IfCodeBlock : SingleControlFlowCodeBlock {
        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new IfInstruction();
        }
    }
}