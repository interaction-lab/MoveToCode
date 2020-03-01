using System;

namespace MoveToCode {
    public class FloatDataType : INumberDataType {

        public FloatDataType(CodeBlock cbIn) : base(cbIn) { }
        public FloatDataType(float valIn) : base(null) {
            SetValue(valIn);
        }

        public override Type GetCastType() {
            return typeof(float);
        }
    }
}
