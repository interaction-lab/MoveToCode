namespace MoveToCode {
    public abstract class IDataType : IArgument {
        public dynamic value;
        protected CodeBlock myCodeBlock;
        public abstract bool IsSameDataTypeAndEqualTo(IDataType otherVal);

        public virtual IDataType EvaluateArgument() {
            return this;
        }

        public virtual CodeBlock GetCodeBlock() {
            return myCodeBlock;
        }

        public virtual dynamic GetValue() {
            return value;
        }

        public virtual void SetCodeBlock(CodeBlock codeBlock) {
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