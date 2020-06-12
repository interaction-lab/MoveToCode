using System;
namespace MoveToCode {
    public abstract class BasicDataType : IDataType {

        public BasicDataType(CodeBlock cbIn) : base(cbIn) { }

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