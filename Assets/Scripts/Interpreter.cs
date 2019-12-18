using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class Interpreter : Singleton<Interpreter> {
        PrintInstruction startingInstruction = new PrintInstruction(new StringDataType("START"));
        Stack<Instruction> instructionStack = new Stack<Instruction>();
        public InstructionReturnValue lastInstructionReturn;
        public Instruction curInstruction;

        private void Awake() {
            ResetAndRunCodeStartUp();
        }

        public void ResetAndRunCodeStartUp() {
            instructionStack.Clear();
            lastInstructionReturn = null;
            curInstruction = startingInstruction;
            // this will also reset memory
        }

        public void RunNextInstruction() {
            if (!CodeIsRunning()) {
                ResetAndRunCodeStartUp();
            }
            lastInstructionReturn = curInstruction.RunInstruction();
            UpdateCurInstruction();
        }

        void UpdateCurInstruction() {
            curInstruction = lastInstructionReturn?.GetNextInstruction();
            if (curInstruction == null) {
                curInstruction = instructionStack.Empty() ?
                    null :
                    instructionStack.Pop();
            }
            if (curInstruction == null) {
                Debug.LogWarning("CODE COMPLETED, not sure what to implement here yet");
            }
        }

        public bool CodeIsRunning() {
            return curInstruction != null;
        }

        public void AddToInstructionStack(Instruction iIn) {
            instructionStack.Push(iIn);
        }
    }
}