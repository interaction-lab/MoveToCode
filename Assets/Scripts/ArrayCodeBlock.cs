using System;

namespace MoveToCode {

    public class ArrayCodeBlock : DataCodeBlock {

        //default
        public int arraySize = 3;

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new ArrayDataStructure(this, arraySize);
        }

        public void SetArraySize(int size) {
            arraySize = size;
        }

        public override void SetOutput(object size) {
            arraySize = Int32.Parse(size.ToString());
        }
    }
}