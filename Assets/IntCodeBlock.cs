namespace MoveToCode {

    public class IntCodeBlock : CodeBlock {
        public string output = "DEFAULT STRING";
        StringDataType stringData;

        private void Awake() {
            stringData = new StringDataType(output);
        }

        protected override IArgument GetArgumentValueOfCodeBlock() {
            return stringData;
        }

        protected override void SetInstructionOrData() {
            myInstruction = null;
        }
    }
}