namespace MoveToCode {
    public class PrintCodeBlock : CodeBlock {
        public string output;

        protected override IArgument GetArgumentValueOfCodeBlock() {
            return myInstruction;
        }

        protected override void SetInstructionOrData() {
            myInstruction = new PrintInstruction(new StringDataType(output));
        }
    }
}