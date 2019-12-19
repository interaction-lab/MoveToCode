namespace MoveToCode {
    public abstract class IDataType : IArgument {
        public dynamic value;
        protected CodeBlock myCodeBlock;

        public IDataType EvaluateArgument() {
            return this;
        }

        public CodeBlock GetCodeBlock() {
            return myCodeBlock;
        }

        public dynamic GetValue() {
            return value;
        }

        public void SetCodeBlock(CodeBlock codeBlock) {
            myCodeBlock = codeBlock;
        }

        public void SetValue(dynamic valIn) {
            value = valIn;
        }
    }
}