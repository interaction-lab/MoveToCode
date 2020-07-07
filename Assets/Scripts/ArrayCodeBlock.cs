namespace MoveToCode {

    public class ArrayCodeBlock : DataCodeBlock {

        //array size
        public int output = 3;

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new ArrayDataStructure(this, output);
        }
    }
}