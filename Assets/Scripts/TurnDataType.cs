using System;

namespace MoveToCode {
    public class TurnDataType : INumberDataType {
        public TurnDataType(CodeBlock cbIn) : base(cbIn) { }
        public TurnDataType(CodeBlock cbIn, CodeBlockEnums.Turn valIn) : base(cbIn) {
            SetValue(valIn);
        }

        public override Type GetCastType() {
            return typeof(CodeBlockEnums.Turn);
        }
    }
}

