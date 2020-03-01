using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class Interpreter : Singleton<Interpreter> {
        Stack<Instruction> instructionStack = new Stack<Instruction>();
        public InstructionReturnValue lastInstructionReturn;
        public Instruction curInstruction;
        public static string instructionRunCol = "InstructionRun";
        int numInstructionsRun = 0;
        public int instructionRunLimit = 30;
        bool fullSteppingCode = false;
        public float stepSpeed = 0.5f;

        private void Awake() {
            ResetCodeState();
            LoggingManager.instance.AddLogColumn(instructionRunCol, "");
        }

        public void ResetCodeState() {
            if (instructionStack == null) {
                instructionStack = new Stack<Instruction>();
            }
            fullSteppingCode = false;
            ExerciseManager.instance.AlertCodeReset();
            ConsoleManager.instance.ClearConsole();
            curInstruction?.GetCodeBlock()?.ToggleOutline(false);
            instructionStack.Clear();
            lastInstructionReturn = null;
            curInstruction = StartCodeBlock.instance.GetMyInternalIArgument() as Instruction;
            StartCodeBlock.instance.ToggleOutline(true);
            CodeBlockManager.instance.ResetAllCodeBlockInternalState();
            MemoryManager.instance.ResetMemoryState();
            numInstructionsRun = 0;
        }

        public void RunNextInstruction() {
            if (!CodeIsRunning()) {
                ResetCodeState();
            }
            else {
                if (curInstruction == StartCodeBlock.instance.GetMyInternalIArgument() as Instruction) {
                    MemoryManager.instance.SaveMemoryState();
                }
                try {
                    ++numInstructionsRun;
                    LoggingManager.instance.UpdateLogColumn(instructionRunCol, curInstruction?.DescriptiveInstructionToString());
                    lastInstructionReturn = curInstruction.RunInstruction();
                    UpdateCurInstruction();
                    if (numInstructionsRun > instructionRunLimit) {
                        throw new Exception("Too man instructions run, maybe an infinite loop?");
                    }
                }
                catch (Exception ex) {
                    string lineToAdd = ex.ToString();
                    if (lineToAdd.Contains("NULL")) {
                        lineToAdd = "Instruction Block Incomplete";
                    }
                    ConsoleManager.instance.AddLine(string.Join("", lineToAdd, ", Code Resetting"));
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
            if (curInstruction != null) {
                curInstruction.GetCodeBlock().ToggleOutline(true);
            }
            else {
                ExerciseManager.instance.AlertCodeFinished();
                ConsoleManager.instance.AddFinishLine();
            }
        }

        public void FullStepCode() {
            StartCoroutine(StepThroughCode());
        }

        IEnumerator StepThroughCode() {
            if (fullSteppingCode) {
                yield break;
            }
            fullSteppingCode = true;
            while (fullSteppingCode && CodeIsRunning()) {
                RunNextInstruction();
                yield return new WaitForSeconds(stepSpeed);
            }
            fullSteppingCode = false;
        }

        public bool CodeIsRunning() {
            return curInstruction != null;
        }

        public void AddToInstructionStack(Instruction iIn) {
            instructionStack.Push(iIn);
        }
    }
}