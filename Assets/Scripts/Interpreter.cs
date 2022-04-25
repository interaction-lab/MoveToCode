using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MoveToCode {
    #region members
    public class Interpreter : Singleton<Interpreter> {
        Stack<Instruction> instructionStack = new Stack<Instruction>();
        public InstructionReturnValue lastInstructionReturn;
        public Instruction curInstruction;
        public static string instructionRunCol = "InstructionRun", codeErrorCol = "codeErrorWhileRunning";
        int numInstructionsRun = 0;
        public int instructionRunLimit = 30;
        bool fullSteppingCode = false;
        public float stepSpeed = 0.5f;
        string kuriOffMaze = "Oh no, baby Kuri fell off the maze!";

        public UnityEvent OnCodeReset, OnCodeStart, OnCodeEnd, OnCodeError;
        #endregion

        #region unity
        private void OnEnable() {
            LoggingManager.instance.AddLogColumn(instructionRunCol, "");
            LoggingManager.instance.AddLogColumn(codeErrorCol, "");
            StartCoroutine(WaitFor1Frame()); // wait for 1 frame or else highlight and events will not work correctly
        }
        IEnumerator WaitFor1Frame() {
            yield return new WaitForEndOfFrame();
            ResetCodeState();
        }
        #endregion

        #region public
        public void ResetCodeState() {
            if (instructionStack == null) {
                instructionStack = new Stack<Instruction>();
            }
            fullSteppingCode = false;
            ExerciseManager.instance.AlertCodeReset();
            ConsoleManager.instance.ClearConsole();
            curInstruction?.GetCodeBlock()?.ToggleOutline(false);
            curInstruction = StartCodeBlock.instance.GetMyIArgument() as Instruction;
            StartCodeBlock.instance.ToggleOutline(true);
            instructionStack.Clear();
            lastInstructionReturn = null;

            CodeBlockManager.instance.ResetAllCodeBlockInternalState();
            MemoryManager.instance.ResetMemoryState();
            numInstructionsRun = 0;
            StaticNextChallengeButton.instance.gameObject.SetActive(false);
            BabyKuriManager.instance.ResetKuri();
            OnCodeReset.Invoke();
        }

        public void RunNextInstruction() {
            if (CodeIsFinished()) {
                ResetCodeState();
            }
            else {
                if (CodeIsAtStart()) {
                    SignalCodeStart();
                }
                try {
                    RunInstruction_Private();
                }
                catch (Exception ex) {
                    CatchCodeError(ex);
                }
            }
        }
        public bool CodeIsRunning() {
            return !CodeIsAtStart() && !CodeIsFinished();
        }

        public bool CodeIsFinished() {
            return curInstruction == null;
        }

        public bool CodeIsAtStart() {
            return curInstruction == (StartCodeBlock.instance.GetMyIArgument() as Instruction);
        }

        public void AddToInstructionStack(Instruction iIn) {
            instructionStack.Push(iIn);
        }

        public void FullStepCode() {
            StartCoroutine(StepThroughCode());
        }
        #endregion

        #region private
        private void CatchCodeError(Exception ex) {
            string lineToAdd = ex.ToString();
            if (lineToAdd.Contains(kuriOffMaze)) {
                lineToAdd = "Rails Error, " + lineToAdd;
                KuriTextManager.instance.Addline("Oh no, moving baby kuri off the maze!");
                Debug.Log("fdskjl");
            }
            else if (lineToAdd.Contains("NULL")) {
                lineToAdd = "Instruction Block Incomplete, " + lineToAdd;
                KuriTextManager.instance.Addline("Code block is incomplete");
            }
            ConsoleManager.instance.AddLine(string.Join("", lineToAdd, ", Code Resetting"));
            LoggingManager.instance.UpdateLogColumn(codeErrorCol, lineToAdd);
            Debug.LogWarning(ex.ToString());
            OnCodeError.Invoke();
        }

        private void RunInstruction_Private() {
            ++numInstructionsRun;
            LoggingManager.instance.UpdateLogColumn(instructionRunCol, curInstruction?.DescriptiveInstructionToString());
            lastInstructionReturn = curInstruction.RunInstruction();
            UpdateCurInstruction();
            if (numInstructionsRun > instructionRunLimit) {
                throw new Exception("Too many instructions run, maybe an infinite loop?");
            }
        }

        private void SignalCodeStart() {
            MemoryManager.instance.SaveMemoryState();
            OnCodeStart.Invoke();
        }

        private void UpdateCurInstruction() {
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
                OnCodeEnd.Invoke();
                if (ExerciseManager.instance.AlertCodeFinished()) {
                    StaticNextChallengeButton.instance.gameObject.SetActive(true);
                }
                ConsoleManager.instance.AddFinishLine();
            }
        }

        IEnumerator StepThroughCode() {
            if (fullSteppingCode) {
                yield break;
            }
            if (CodeIsFinished()) {
                ResetCodeState();
                yield break;
            }
            fullSteppingCode = true;
            while (fullSteppingCode && !CodeIsFinished()) {
                RunNextInstruction();
                yield return new WaitForSeconds(stepSpeed);
                if (BabyKuriManager.instance.KuriIsOffRails) {
                    BabyKuriManager.instance.KuriIsOffRails = false;
                    fullSteppingCode = false;
                    CatchCodeError(new Exception(kuriOffMaze));
                }
            }
            fullSteppingCode = false;
        }
        #endregion
    }
}