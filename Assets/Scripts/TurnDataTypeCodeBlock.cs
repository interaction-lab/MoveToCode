namespace MoveToCode {

    public class TurnDataTypeCodeBlock : DataCodeBlock {
        public CodeBlockEnums.Turn output;

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new TurnDataType(this, output);
        }

        public override void SetOutput(object value) {
            output = (CodeBlockEnums.Turn)value;
        }
    }
}