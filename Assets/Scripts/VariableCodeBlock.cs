
using UnityEngine;

namespace MoveToCode {
    public class VariableCodeBlock : DataCodeBlock {

        VariableBlockCollection parentVariableBlockCollection;

        // add itself to a collection
        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new Variable();
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
