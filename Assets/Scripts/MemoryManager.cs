using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MemoryManager : Singleton<MemoryManager> {
        Dictionary<Variable, IDataType> variables;
        protected CodeBlock myCodeBlock;

        private void Awake() {
            variables = new Dictionary<Variable, IDataType>();
        }

        public IDataType GetVariableValue(Variable vIn) {
            return variables[vIn];
        }

        public void AddVariable(Variable vIn, IDataType dIn) {
            variables.Add(vIn, dIn);
        }

        public CodeBlock GetCodeBlock() {
            return myCodeBlock;
        }

        public void SetCodeBlock(CodeBlock codeBlock) {
            myCodeBlock = codeBlock;
        }
    }
}