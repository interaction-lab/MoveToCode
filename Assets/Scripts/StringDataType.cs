using System;

namespace MoveToCode {
    public class StringDataType : IDataType {
        public StringDataType(string valIn) {
            SetValue(valIn);
        }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is StringDataType) {
                return (string)GetValue() == (string)otherVal.GetValue();
            }
            throw new InvalidOperationException("Trying to compare String to a non String Data Type");
        }
    }
}
