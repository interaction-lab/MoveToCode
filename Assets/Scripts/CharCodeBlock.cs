namespace MoveToCode {
    public class CharCodeBlock : DataCodeBlock {
        //default
        public char output = '0';

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new CharDataType(this, output);
        }
    }
}