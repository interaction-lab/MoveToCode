namespace MoveToCode {
    public class IfElseCodeBlock : DoubleControlFlowCodeBlock {
        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new IfElseInstruction();
        }
    }
}