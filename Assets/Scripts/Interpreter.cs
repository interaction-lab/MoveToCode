using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class Interpreter : Singleton<Interpreter> {
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
            ConsoleManager.instance.ClearConsole();
            curInstruction?.GetCodeBlock()?.ToggleOutline(false);
            instructionStack.Clear();
            lastInstructionReturn = null;
            curInstruction = StartCodeBlock.instance.GetMyInternalIArgument() as Instruction;
            StartCodeBlock.instance.ToggleOutline(true);
            CodeBlockManager.instance.ResetAllCodeBlockInternalState();
            MemoryManager.instance.ResetMemoryState();
        }

        public void RunNextInstruction() {
            if (!CodeIsRunning()) {
                ResetCodeState();
                MemoryManager.instance.SaveMemoryState();
            }
            else {
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
        }

        void UpdateCurInstruction() {
            curInstruction?.GetCodeBlock()?.ToggleOutline(false);
            curInstruction = lastInstructionReturn?.GetNextInstruction();
            if (curInstruction == null) {
                curInstruction = instructionStack.Empty() ?
                    null :
                    instructionStack.Pop();
            }
            if (curInstruction == null) {
                StartCodeBlock.instance.ToggleOutline(true);
            }
            else {
                curInstruction?.GetCodeBlock()?.ToggleOutline(true);
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