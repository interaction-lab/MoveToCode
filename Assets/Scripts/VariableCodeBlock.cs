
namespace MoveToCode {
    public class VariableCodeBlock : DataCodeBlock {

        VariableBlockCollection parentVariableBlockCollection;
        Variable myVariable;
        // add itself to a collection
        protected override void SetInstructionOrData() {
            myVariable = new Variable();
        }

        public IDataType GetVariableValueFromBlockCollection() {
            return parentVariableBlockCollection?.GetVariableValueFromBlockCollection();
        }

        public void SetVariableValueFromBlockCollection(IDataType valIn) {
            parentVariableBlockCollection.SetVariableValueFromBlockCollection(valIn);
        }

        public override string ToString() {
            return GetVariableValueFromBlockCollection()?.GetValue()?.ToString();
        }
    }
}
