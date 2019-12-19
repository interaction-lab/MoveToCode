namespace MoveToCode {
    public class PrintCodeBlock : CodeBlock {
        public string output;

        protected override void SetInstructionOrData() {
            myInstruction = new PrintInstruction(new StringDataType(output));
        }
    }
}