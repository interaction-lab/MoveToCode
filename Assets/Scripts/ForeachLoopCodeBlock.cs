namespace MoveToCode {
    public class ForeachLoopCodeBlock : SingleControlFlowCodeBlock {
        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new ForeachLoopInstruction(this);
        }
    }
}