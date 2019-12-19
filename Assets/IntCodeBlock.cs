namespace MoveToCode {

    public class IntCodeBlock : CodeBlock {
        public int output;

        protected override void SetInstructionOrData() {
            myData = new IntDataType(output);
        }
    }
}