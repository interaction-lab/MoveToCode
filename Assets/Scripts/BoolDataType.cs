using System;

namespace MoveToCode {
    public class BoolDataType : IDataType {
        public BoolDataType(bool valIn) {
            SetValue(valIn);
        }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is BoolDataType) {
                return (bool)GetValue() == (bool)otherVal.GetValue();
            }
            throw new InvalidOperationException("Trying to compare Bool to a non bool Data Type");
        }
    }
}
