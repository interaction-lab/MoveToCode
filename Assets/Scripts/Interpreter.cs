using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class Interpreter : Singleton<Interpreter> {
        PrintInstruction startingInstruction = new PrintInstruction(new StringDataType("START"));
        Stack<Instruction> instructionStack = new Stack<Instruction>();
        public InstructionReturnValue lastInstructionReturn;
        public Instruction curInstruction;

        Color lastColor;

        private void Awake() {
            ResetCodeState();
        }

        public void ResetCodeState() {
            if (instructionStack == null) {
                instructionStack = new Stack<Instruction>();
            }
            instructionStack.Clear();
            lastInstructionReturn = null;
            curInstruction = startingInstruction;
            // this will also reset memory later
        }

        public void RunNextInstruction() {
            if (!CodeIsRunning()) {
                ResetCodeState();
            }
            try {
                lastInstructionReturn = curInstruction.RunInstruction();
                UpdateCurInstruction();
            }
            catch (Exception ex) {
                Debug.LogWarning("Exception caught while running code: ");
                Debug.LogWarning(ex.ToString());
                ResetCodeState();
            }

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