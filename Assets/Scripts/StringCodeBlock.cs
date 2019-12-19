namespace MoveToCode {

    public class StringCodeBlock : CodeBlock {
        public string output = "DEFAULT";

        protected override void SetInstructionOrData() {
            myData = new StringDataType(output);
        }
    }
}