namespace MoveToCode {

    public class ArrayCodeBlock : DataCodeBlock {

        public int arraySize = 3;

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new ArrayDataStructure(this, arraySize);
        }
    }
}