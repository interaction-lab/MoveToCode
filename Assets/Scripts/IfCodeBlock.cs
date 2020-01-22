namespace MoveToCode {
    public class IfCodeBlock : ControlFlowCodeBlock {

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new IfInstruction();
        }
    }
}