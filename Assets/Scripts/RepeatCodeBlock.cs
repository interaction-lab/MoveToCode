namespace MoveToCode {
    public class RepeatCodeBlock : SingleControlFlowCodeBlock {
        protected override void SetMyBlockInternalArg() {
            if (myBlockInternalArg == null) {
                myBlockInternalArg = new RepeatInstruction(this);
            }
        }
    }
}