namespace MoveToCode {

    public class ElementCodeBlock : DataCodeBlock {

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new IntDataType(this, 1);
        }
    }
}