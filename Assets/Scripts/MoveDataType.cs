using System;

namespace MoveToCode {
    public class MoveDataType : EnumDataType {
        public MoveDataType(CodeBlock cbIn) : base(cbIn) { }
        public MoveDataType(CodeBlock cbIn, CodeBlockEnums.Move valIn) : base(cbIn) {
            SetValue(valIn);
        }

        public override Type GetCastType() {
            return typeof(CodeBlockEnums.Move);
        }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            return (CodeBlockEnums.Move)otherVal.GetValue() == (CodeBlockEnums.Move)value;
        }
    }
}

