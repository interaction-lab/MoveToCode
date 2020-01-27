﻿namespace MoveToCode {
    public abstract class ControlFlowInstruction : Instruction {
        protected bool conditionIsTrue;
        protected bool exitInstructionAddedToStack;
        protected Instruction exitInstruction;

        public override void EvaluateArgumentList() {
            conditionIsTrue = (argumentList[0] as ConditionalInstruction)?.RunInstruction().GetReturnDataVal().GetValue();
        }

        public override void SetUpArgumentCompatability() {
            compatibileArgumentTypes.Add(typeof(ConditionalInstruction));
        }

        public override int GetNumArguments() {
            return 1;
        }

        public ControlFlowInstruction() {
            exitInstructionAddedToStack = false;
        }

        public Instruction GetExitInstruction() {
            return exitInstruction;
        }
        public void SetExitInstruction(Instruction iIn) {
            exitInstruction = iIn;
        }

        public int FindExitChainSize() {
            Instruction runner = GetExitInstruction();
            if (runner != null) {
                return runner.GetCodeBlock().FindChainSize() + 1;
            }
            return 0;
        }

    }
}
