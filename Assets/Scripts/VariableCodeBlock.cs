
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
            parentVariableBlockCollection.SetVariableValue(valIn);
        }

        public string GetVariableNameFromBlockCollection() {
            return parentVariableBlockCollection?.GetVariableNameFromBlockCollection();
        }

        public void SetParentBlockCollection(VariableBlockCollection vBCIn) {
            parentVariableBlockCollection = vBCIn;
        }

        public override string ToString() {
            return GetVariableNameFromBlockCollection();
        }
    }
}
