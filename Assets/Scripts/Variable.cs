using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class Variable : IArgument {
        public IDataType EvaluateArgument() {
            return MemoryManager.instance.GetVariableValue(this);
        }

        public CodeBlock GetCodeBlock() {
            throw new System.NotImplementedException();
        }

        public void SetCodeBlock(CodeBlock codeBlock) {
            throw new System.NotImplementedException();
        }
    }
}
