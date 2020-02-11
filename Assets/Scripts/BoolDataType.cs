using System;

namespace MoveToCode {
    public class BoolDataType : IDataType {
        public BoolDataType(CodeBlock cbIn) : base(cbIn) { }
        public BoolDataType(CodeBlock cbIn, bool valIn) : base(cbIn, valIn) { }
        public BoolDataType(bool valIn) : base(null, valIn) { }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is BoolDataType) {
                return (bool)GetValue() == (bool)otherVal.GetValue();
            }
            throw new InvalidOperationException("Trying to compare Bool to a non bool Data Type");
        }
    }
}
