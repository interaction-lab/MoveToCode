using System;

namespace MoveToCode {
    public class Variable : IDataType {

        VariableCodeBlock myVariableCodeBlock;

        public Variable(CodeBlock cbIn) : base(cbIn) {
            myVariableCodeBlock = cbIn as VariableCodeBlock;
        }

        public override IDataType EvaluateArgument() {
            return myVariableCodeBlock.GetVariableDataFromBlockCollection();
        }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            return GetMyData().IsSameDataTypeAndEqualTo(otherVal);
        }

        public void SetValue(IDataType dIn) {
            myVariableCodeBlock.SetVariableValueFromBlockCollection(dIn);
        }

        public override object GetValue() {
            return GetMyData().GetValue();
        }
        public IDataType GetMyData() {
            return myVariableCodeBlock.GetVariableDataFromBlockCollection();
        }

        public override string ToString() {
            return string.Join("", myVariableCodeBlock.GetVariableNameFromBlockCollection(), " : ", myVariableCodeBlock.GetVariableDataFromBlockCollection().DescriptiveInstructionToString());
        }

        public override Type GetCastType() {
            return GetMyData().GetCastType();
        }
    }
}
