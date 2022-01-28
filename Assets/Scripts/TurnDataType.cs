using System;

namespace MoveToCode {
    public class TurnDataType : EnumDataType {
        public TurnDataType(CodeBlock cbIn) : base(cbIn) { }
        public TurnDataType(CodeBlock cbIn, CodeBlockEnums.Turn valIn) : base(cbIn) {
            SetValue(valIn);
        }

        public override Type GetCastType() {
            return typeof(CodeBlockEnums.Turn);
        }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            return (CodeBlockEnums.Turn)otherVal.GetValue() == (CodeBlockEnums.Turn)value;
        }
    }
}

