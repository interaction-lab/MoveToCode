using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MemoryManager : Singleton<MemoryManager> {
        Dictionary<string, VariableBlockCollection> variables;

        private void Awake() {
            variables = new Dictionary<string, VariableBlockCollection>();
        }

        public void AddNewVariableCodeBlock(string varName, VariableCodeBlock cbIn, IDataType dIn = null) {
            if (variables.ContainsKey(varName)) {
                variables[varName].AddCodeBlock(cbIn);
            }
            else {
                variables[varName] = new VariableBlockCollection(dIn);
            }
        }

    }
}