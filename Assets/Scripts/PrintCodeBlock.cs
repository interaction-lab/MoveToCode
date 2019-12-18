namespace MoveToCode {
    public class PrintCodeBlock : CodeBlock {
        public string output;
        private void Awake() {
            myInstruction = new PrintInstruction(new StringDataType(output));
        }
    }
}