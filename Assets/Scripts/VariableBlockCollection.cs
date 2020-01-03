using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class VariableBlockCollection : MonoBehaviour {
        HashSet<VariableCodeBlock> variableCodeBlockSet;
        IDataType myData;

        public VariableBlockCollection(IDataType dIn) {
            SetVariableValueFromBlockCollection(dIn);
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

    }
}