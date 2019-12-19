namespace MoveToCode {

    public class IntCodeBlock : CodeBlock {
        public string output = "DEFAULT STRING";

        protected override void SetInstructionOrData() {
            myData = new StringDataType(output);
        }
    }
}