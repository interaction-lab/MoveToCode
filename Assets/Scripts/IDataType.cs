namespace MoveToCode {
    public abstract class IDataType : IArgument {
        public dynamic value;
        protected CodeBlock myCodeBlock;
        public abstract bool IsSameDataTypeAndEqualTo(IDataType otherVal);

        public IDataType(CodeBlock cbIn) : base(cbIn) { }
        public IDataType(CodeBlock cbIn, dynamic valIn) : base(cbIn) {
            SetValue(valIn);
        }

        public override IDataType EvaluateArgument() {
            return this;
        }

        public virtual dynamic GetValue() {
            return value;
        }

        public virtual void SetValue(dynamic valIn) {
            value = valIn;
        }

        public override string ToString() {
            return GetValue().ToString();
        }

        public override int GetNumArguments() {
            return 0;
        }
    }
}