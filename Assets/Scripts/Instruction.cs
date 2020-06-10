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
        /*
        public List<Type> GetArgCompatibilityAtPos(int pos) {
            if (argPosToCompatability == null) {
                SetUpArgPosToCompatability();
            }
            return argPosToCompatability[pos];
        }

        public IArgument GetArgumentAt(int pos) {
            return GetArgumentListAsIArgs()[pos];
        }

        public List<IArgument> GetArgumentListAsIArgs() {
            return GetCodeBlock().GetArgumentListAsIArgs();
        }

        public List<string> GetArgListDescription() {
            if (argDescriptionList == null) {
                SetUpArgDescriptionList();
            }
            return argDescriptionList;
        }

        public void PrintArgDescriptions() {
            int i = 0;
            foreach (string s in GetArgListDescription()) {
                Debug.Log(string.Join("", "Arg[", i.ToString(), "] = ", s));
                ++i;
            }
        }
    }*/
    }
}