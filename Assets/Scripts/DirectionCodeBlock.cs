using System;

namespace MoveToCode {

    public class DirectionCodeBlock : DataCodeBlock {
        public DirectionDataType.DIRECTION output;

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new DirectionDataType(this, output);
        }

        public override void SetOutput(object value) {
            output = (DirectionDataType.DIRECTION)value;
        }
    }
}