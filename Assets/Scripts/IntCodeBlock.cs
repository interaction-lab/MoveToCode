namespace MoveToCode {

    public class IntCodeBlock : DataCodeBlock {
        public int output;

        protected override void SetInstructionOrData() {
            myData = new IntDataType(output);
        }
    }
}