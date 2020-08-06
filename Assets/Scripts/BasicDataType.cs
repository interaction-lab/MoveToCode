using System;
using UnityEngine;

namespace MoveToCode {
    public abstract class BasicDataType : IDataType {

        public BasicDataType(CodeBlock cbIn) : base(cbIn) { }

        public override string ToString() {
            return Convert.ChangeType(GetValue(), GetCastType()).ToString();
        }

        public override string DescriptiveInstructionToString() {
            return ToString();
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