using System;

namespace MoveToCode {
    public class IntDataType : INumberDataType {
        public IntDataType(CodeBlock cbIn) : base(cbIn) { }
        public IntDataType(CodeBlock cbIn, int valIn) : base(cbIn) {
            SetValue(valIn);
        }

        public override Type GetCastType() {
            return typeof(int);
        }
    }
}

