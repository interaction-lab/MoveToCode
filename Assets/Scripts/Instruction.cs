using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class Instruction : IArgument {

        Instruction nextInstruction;
        protected List<IArgument> argumentList = new List<IArgument>();
        protected HashSet<Type> compatibileArgumentTypes;
        protected CodeBlock myCodeBlock;

        public abstract InstructionReturnValue RunInstruction();
        public abstract void SetUpArgumentCompatability();
        public abstract void EvaluateArgumentList();
        public abstract int GetNumArguments();

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

        public CodeBlock GetCodeBlock() {
            return myCodeBlock;
        }

        public void SetCodeBlock(CodeBlock codeBlock) {
            myCodeBlock = codeBlock;
        }

        public void TryArgumentCompatibleType(Type argTypeIn) {
            // need to check for variable too
            if (compatibileArgumentTypes == null || compatibileArgumentTypes.Empty()) {
                compatibileArgumentTypes = new HashSet<Type>();
                compatibileArgumentTypes.Add(null);
                SetUpArgumentCompatability();
            }
            foreach (Type T in compatibileArgumentTypes) {
                if (T == null) {
                    if (argTypeIn == null) {
                        return;
                    }
                }
                else if (argTypeIn.IsAssignableFrom(T) || T.IsAssignableFrom(argTypeIn)) {
                    return;
                }
            }
            throw new InvalidOperationException("Argument type invalid, cannot use");

        }

        public void SetArgumentAt(IArgument argIn, int position) {
            Type typeToTry = argIn as Variable != null ?
                (argIn as Variable).GetMyData().GetType() :
                argIn?.GetType();
            TryArgumentCompatibleType(typeToTry);
            argumentList[position] = argIn;
        }

        public void ResizeArgumentList(int desiredSize) {
            argumentList.Resize(desiredSize);
        }

        public IDataType EvaluateArgument() {
            return RunInstruction().GetReturnDataVal();
        }
    }
}