using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class CharCodeBlock : DataCodeBlock {
        //default
        public char output = '0';

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new CharDataType(this, output);
        }

        public override void SetOutput(object value) {
            output = Convert.ToChar(value);
        }
    }
}