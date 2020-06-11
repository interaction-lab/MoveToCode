using System;

namespace MoveToCode {
    public class CharDataType : BasicDataType {
        public CharDataType(CodeBlock cbIn) : base(cbIn) { }
        public CharDataType(CodeBlock cbIn, char valIn) : base(cbIn) { SetValue(valIn); }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is CharDataType) {
                return (char)GetValue() == (char)(otherVal as CharDataType).GetValue();
            }
            throw new InvalidOperationException("Trying to compare char to a non char Data Type");
        }

        public override string ToString() {
            return string.Join("", "'", base.ToString(), "'");
        }

        public override Type GetCastType() {
            return typeof(char);
        }
    }
}
