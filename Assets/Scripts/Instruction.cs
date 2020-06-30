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

        public string ReplaceFirst(string text, string find, string replace)
        { //replace first occurence of 'find' with 'replace'
            int pos = text.IndexOf(find);
            if (pos < 0) return text;
            return text.Substring(0, pos) + replace + text.Substring(pos + find.Length);
        }
    }
}