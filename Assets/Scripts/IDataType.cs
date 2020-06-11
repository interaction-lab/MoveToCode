using System;
namespace MoveToCode {
    public abstract class IDataType : IArgument {
        public object value;
        public abstract Type GetCastType();
        protected CodeBlock myCodeBlock;
        public abstract bool IsSameDataTypeAndEqualTo(IDataType otherVal);

        public IDataType(CodeBlock cbIn) : base(cbIn) { }

        public virtual object GetValue() {
            return value;
        }

        public override int GetNumArguments() {
            return 0;
        }

        public override IDataType EvaluateArgument() {
            return this;
        }
    }
}