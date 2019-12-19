using System.Collections.Generic;

namespace MoveToCode {
    public abstract class Instruction : IArgument {

        Instruction nextInstruction;
        protected List<IArgument> argumentList = new List<IArgument>();
        protected CodeBlock myCodeBlock;

        public abstract InstructionReturnValue RunInstruction();
        public abstract void EvaluateArgumentList();
        public abstract int GetNumArguments();

        public void SetNextInstruction(Instruction instIn) {
            nextInstruction = instIn;
        }
        public Instruction GetNextInstruction() {
            return nextInstruction;
        }
        public IArgument GetArgumentAt(int position) {
            return argumentList[position];
        }
        public void RemoveArgumentAt(int position) {
            argumentList[position] = null;
        }

        public CodeBlock GetCodeBlock() {
            return myCodeBlock;
        }

        public void SetCodeBlock(CodeBlock codeBlock) {
            myCodeBlock = codeBlock;
        }

        /// <summary>
        /// Adds argument onto the argument list at specified position. If position is -1, argument is added to end of list. Resizes list if index out of bounds.
        /// </summary>
        /// <param name="argIn">Argument to be added to the list.</param>
        /// <param name="position">Position of the argument, zero indexed. Default value -1, argument is added to end in this case. </param>
        public void SetArgumentAt(IArgument argIn, int position = -1) {
            ResizeArgumentList(position + 1);
            if (position == -1) {
                argumentList.Add(argIn);
            }
            else {
                argumentList[position] = argIn;
            }
        }

        public void ResizeArgumentList(int desiredSize) {
            argumentList.Resize(desiredSize);
        }

        public IDataType EvaluateArgument() {
            return RunInstruction().GetReturnDataVal();
        }
    }
}