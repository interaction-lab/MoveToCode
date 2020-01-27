namespace MoveToCode {
    public abstract class IDataType : IArgument {
        public dynamic value;
        protected CodeBlock myCodeBlock;
        public abstract bool IsSameDataTypeAndEqualTo(IDataType otherVal);

        public override IDataType EvaluateArgument() {
            return this;
        }

        public override CodeBlock GetCodeBlock() {
            return myCodeBlock;
        }

        public virtual dynamic GetValue() {
            return value;
        }

        public override void SetCodeBlock(CodeBlock codeBlock) {
            myCodeBlock = codeBlock;
        }

        public virtual void SetValue(dynamic valIn) {
            value = valIn;
        }

        public override string ToString() {
            return GetValue().ToString();
        }
    }
}