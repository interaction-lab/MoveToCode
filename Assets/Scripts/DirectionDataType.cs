using System;

namespace MoveToCode {
    public class DirectionDataType : BasicDataType {
        public enum DIRECTION {
            NULL,
            Up,
            Down,
            Left,
            Right
        }

        public DirectionDataType(CodeBlock cbIn) : base(cbIn) { }
        public DirectionDataType(CodeBlock cbIn, DIRECTION valIn) : base(cbIn) {
            SetValue(valIn);
        }

        public override Type GetCastType() {
            return typeof(DIRECTION);
        }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is DirectionDataType) {
                return (DIRECTION)otherVal.value == (DIRECTION)value;
            }
            throw new InvalidOperationException("Trying to compare a Direction Type to a non Direction Data Type");
        }
    }
}
