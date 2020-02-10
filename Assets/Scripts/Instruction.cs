using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MoveToCode {
    public abstract class Instruction : IArgument {
        protected CodeBlock myCodeBlock;
        protected List<List<Type>> argPosToCompatability;
        protected List<string> argDescriptionList;

        public abstract InstructionReturnValue RunInstruction();
        public abstract void EvaluateArgumentList();
        public abstract void SetUpArgPosToCompatability();
        public abstract void SetUpArgDescriptionList();

        // public methods, all codeblocks should create instructions with themself
        public Instruction(CodeBlock cbIn) {
            myCodeBlock = cbIn;
        }

        public override CodeBlock GetCodeBlock() {
            Assert.IsNotNull(myCodeBlock);
            return myCodeBlock;
        }

        public override IDataType EvaluateArgument() {
            return RunInstruction().GetReturnDataVal();
        }

        public List<Type> GetArgCompatibilityAtPos(int pos) {
            if (argPosToCompatability == null) {
                SetUpArgPosToCompatability();
            }
            return argPosToCompatability[pos];
        }

        public void List<IArgument> GetArgumentList(){
            return GetCodeBlock().GetArgumentList();
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
    }
}