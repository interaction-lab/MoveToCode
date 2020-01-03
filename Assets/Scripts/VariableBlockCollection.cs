using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class VariableBlockCollection : MonoBehaviour {
        HashSet<VariableCodeBlock> variableCodeBlockSet;
        IDataType myData;
        string variableName;

        public VariableBlockCollection(IDataType dIn, string vNameIn) {
            SetVariableValueFromBlockCollection(dIn);
            variableName = vNameIn;
        }

        public IDataType GetVariableValueFromBlockCollection() {
            return myData;
        }

        public void SetVariableValueFromBlockCollection(IDataType valIn) {
            myData = valIn;
        }

        public void AddCodeBlock(VariableCodeBlock cbIn) {
            variableCodeBlockSet.Add(cbIn);
        }
        public string GetVariableNameFromBlockCollection() {
            return variableName;
        }
    }
}