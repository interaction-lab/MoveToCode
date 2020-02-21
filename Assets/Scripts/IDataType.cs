using System;
namespace MoveToCode {
    public abstract class IDataType : IArgument {
        public object value;
        public abstract Type GetCastType();
        protected CodeBlock myCodeBlock;
        public abstract bool IsSameDataTypeAndEqualTo(IDataType otherVal);

        public IDataType(CodeBlock cbIn) : base(cbIn) { }


        public override IDataType EvaluateArgument() {
            return this;
        }

        public virtual object GetValue() {
            return value;
        }

        public virtual void SetValue(object valIn) {
            value = valIn;
        }

        public override string ToString() {
            return Convert.ChangeType(GetValue(), GetCastType()).ToString();
        }

        public override int GetNumArguments() {
            return 0;
        }

        public T CastObject<T>(object input) {
            return (T)input;
        }

        public T ConvertObject<T>(object input) {
            return (T)Convert.ChangeType(input, typeof(T));
        }
    }
}