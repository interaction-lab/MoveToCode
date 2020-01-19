namespace MoveToCode {
    public class Variable : IDataType {

        VariableCodeBlock myVariableCodeBlock;

        public override IDataType EvaluateArgument() {
            return myVariableCodeBlock.GetVariableDataFromBlockCollection();
        }

        public override CodeBlock GetCodeBlock() {
            return myVariableCodeBlock;
        }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            return GetMyData().IsSameDataTypeAndEqualTo(otherVal);
        }

        public override void SetCodeBlock(CodeBlock codeBlock) {
            myVariableCodeBlock = codeBlock as VariableCodeBlock;
        }

        public void SetValue(IDataType dIn) {
            myVariableCodeBlock.SetVariableValueFromBlockCollection(dIn);
        }
        public override dynamic GetValue() {
            return GetMyData().GetValue();
        }
        public IDataType GetMyData() {
            return myVariableCodeBlock.GetVariableDataFromBlockCollection();
        }

        public override string ToString() {
            return myVariableCodeBlock.GetVariableNameFromBlockCollection();
        }
    }
}
