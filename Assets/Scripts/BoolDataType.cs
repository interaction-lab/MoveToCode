using System;

namespace MoveToCode {
    public class BoolDataType : IDataType {
        public BoolDataType(CodeBlock cbIn) : base(cbIn) { }
        public BoolDataType(CodeBlock cbIn, bool valIn) : base(cbIn) { SetValue(valIn); }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is BoolDataType) {
                return (bool)GetValue() == (bool)otherVal.GetValue();
            }
            throw new InvalidOperationException("Trying to compare Bool to a non bool Data Type");
        }
        public override Type GetCastType() {
            return typeof(bool);
        }
    }
}
