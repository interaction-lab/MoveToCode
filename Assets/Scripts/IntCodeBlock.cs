namespace MoveToCode {

    public class IntCodeBlock : DataCodeBlock {
        //public int output;

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new IntDataType(this, (int)output);
        }
    }
}