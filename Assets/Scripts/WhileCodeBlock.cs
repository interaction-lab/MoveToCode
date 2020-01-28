namespace MoveToCode {
    public class WhileCodeBlock : SingleControlFlowCodeBlock {
        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new WhileInstruction();
        }
    }
}