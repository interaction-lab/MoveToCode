using System;

namespace MoveToCode {

    public class IntCodeBlock : DataCodeBlock {
        public int output;

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new IntDataType(this, output);
        }

        public override void SetOutput(object value) {
            output = Int32.Parse(value.ToString());
            base.SetOutput(value);
        }
    }
}