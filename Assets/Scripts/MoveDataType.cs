using System;

namespace MoveToCode {
    public class MoveDataType : INumberDataType {
        public MoveDataType(CodeBlock cbIn) : base(cbIn) { }
        public MoveDataType(CodeBlock cbIn, CodeBlockEnums.Move valIn) : base(cbIn) {
            SetValue(valIn);
        }

        public override Type GetCastType() {
            return typeof(CodeBlockEnums.Move);
        }
    }
}

