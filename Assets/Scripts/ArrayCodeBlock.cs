namespace MoveToCode {

    public class ArrayCodeBlock : DataCodeBlock {
        public int size = 3;

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new ArrayDataStructure(this, size);
        }

    }
}