namespace MoveToCode {

    public class StringCodeBlock : DataCodeBlock {
        public string output = "DEFAULT";

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new StringDataType(this, output);
        }

        public override void SetOutput(object value) {
            output = value.ToString();
        }
    }
}