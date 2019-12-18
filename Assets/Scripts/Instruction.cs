using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public abstract class Instruction : IArgument {
        public List<IArgument> argumentList = new List<IArgument>();
        public abstract InstructionReturnValue RunInstruction();
        public abstract void EvaluateArgumentList();
        Instruction nextInstruction;

        public void SetNextInstruction(Instruction instIn) {
            nextInstruction = instIn;
        }
        public Instruction GetNextInstruction() {
            return nextInstruction;
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
            while (desiredSize > argumentList.Count) {
                argumentList.Add(new NULLArgument());
            }
        }

        public IDataType EvaluateArgument() {
            return RunInstruction().GetReturnDataVal();
        }
    }
}