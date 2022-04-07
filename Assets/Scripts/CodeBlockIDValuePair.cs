namespace MoveToCode {
    public class CodeBlockIDValuePair {
        public CodeBlockIDValuePair() { }

        public string codeBlockID;
        public object value;

        public string GetCodeBlockID() {
            return codeBlockID;
        }

        public object GetValue() {
            return value;
        }
    }
}

