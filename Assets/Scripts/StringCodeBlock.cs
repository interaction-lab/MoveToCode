namespace MoveToCode {

    public class StringCodeBlock : DataCodeBlock {
        public string output = "DEFAULT";

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new StringDataType(this, output);
        }

    }
}