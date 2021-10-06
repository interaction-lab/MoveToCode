namespace MoveToCode {
    public class ArrayIndexCodeBlock : InstructionCodeBlock {

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new ArrayIndexInstruction(this);
        }
    }
}