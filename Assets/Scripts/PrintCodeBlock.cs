namespace MoveToCode {
    public class PrintCodeBlock : InstructionCodeBlock {
        public string output;

        protected override void SetInstructionOrData() {
            myInstruction = new PrintInstruction(new StringDataType(output));
        }
    }
}