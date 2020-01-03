using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class Variable : IArgument {

        VariableCodeBlock myVariableCodeBlock;

        public IDataType EvaluateArgument() {
            return myVariableCodeBlock.GetVariableValueFromBlockCollection();
        }

        public CodeBlock GetCodeBlock() {
            return myVariableCodeBlock;
        }

        public void SetCodeBlock(CodeBlock codeBlock) {
            myVariableCodeBlock = codeBlock as VariableCodeBlock;
        }
    }
}
