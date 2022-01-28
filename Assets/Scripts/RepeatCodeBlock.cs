namespace MoveToCode {
    public class RepeatCodeBlock : SingleControlFlowCodeBlock {
        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new RepeatInstruction(this);
        }
    }
}