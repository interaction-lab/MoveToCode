namespace MoveToCode {

    public class StringCodeBlock : DataCodeBlock {
        public string output = "DEFAULT";

        protected override void SetInstructionOrData() {
            myData = new StringDataType(output);
        }

        public override string ToString() {
            return myData.ToString();
        }
    }
}