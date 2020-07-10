using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public abstract class Instruction : IArgument {
        public abstract InstructionReturnValue RunInstruction();

        // public methods, all codeblocks should create instructions with themself
        public Instruction(CodeBlock cbIn) : base(cbIn) { }

        public override IDataType EvaluateArgument() {
            return RunInstruction().GetReturnDataVal();
        }
    }
}