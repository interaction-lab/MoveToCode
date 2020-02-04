using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class Instruction : IArgument {

        Instruction nextInstruction;
        protected List<IArgument> argumentList = new List<IArgument>();
        protected CodeBlock myCodeBlock;

        public abstract InstructionReturnValue RunInstruction();
        public abstract void EvaluateArgumentList();
        public abstract int GetNumArguments();
        public abstract List<Type> GetArgCompatibilityAtPos(int pos);

        // public methods
        public Instruction() {
            ResizeArgumentList(GetNumArguments());
        }

        public void SetNextInstruction(Instruction instIn) {
            nextInstruction = instIn;
        }
        public Instruction GetNextInstruction() {
            return nextInstruction;
        }
        public IArgument GetArgumentAt(int position) {
            return argumentList[position];
        }

        public override CodeBlock GetCodeBlock() {
            return myCodeBlock;
        }

        public override void SetCodeBlock(CodeBlock codeBlock) {
            myCodeBlock = codeBlock;
        }

        public void SetArgumentAt(IArgument argIn, int position) {
            argumentList[position] = argIn;
        }

        public void ResizeArgumentList(int desiredSize) {
            argumentList.Resize(desiredSize);
        }

        public override IDataType EvaluateArgument() {
            return RunInstruction().GetReturnDataVal();
        }
    }
}