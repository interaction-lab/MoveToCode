using System;

namespace MoveToCode {
    public class StringDataType : IDataType {
        public StringDataType(CodeBlock cbIn) : base(cbIn) { }
        public StringDataType(CodeBlock cbIn, string valIn) : base(cbIn) {
            SetValue(valIn);
        }
        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is StringDataType) {
                return (string)GetValue() == (string)otherVal.GetValue();
            }
            throw new InvalidOperationException("Trying to compare String to a non String Data Type");
        }

        public override string ToString() {
            return string.Join("", "\"", base.ToString(), "\"");
        }

        public override Type GetCastType() {
            return typeof(string);
        }
    }
}
