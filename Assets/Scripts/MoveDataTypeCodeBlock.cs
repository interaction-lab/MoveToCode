using System;

namespace MoveToCode {

    public class MoveDataTypeCodeBlock : DataCodeBlock {
        public CodeBlockEnums.Move output;

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new MoveDataType(this, output);
        }

        public override void SetOutput(object value) {
            output = (CodeBlockEnums.Move)value;
        }
    }
}