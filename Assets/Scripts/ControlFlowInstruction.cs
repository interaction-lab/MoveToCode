using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class ControlFlowInstruction : StandAloneInstruction {
        protected bool conditionIsTrue;
        protected bool exitInstructionAddedToStack;
        protected Instruction exitInstruction;

        public override void ResestInternalState() {
            exitInstructionAddedToStack = false;
        }

        public override void EvaluateArgumentList() {
            conditionIsTrue = (argumentList[0] as ConditionalInstruction)?.RunInstruction().GetReturnDataVal().GetValue();
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

    }
}
