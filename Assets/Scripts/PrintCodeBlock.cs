namespace MoveToCode {
    public class PrintCodeBlock : InstructionCodeBlock {
        public string output;

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new PrintInstruction(new StringDataType(output));
        }
    }
}