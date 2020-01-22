using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public abstract class ControlFlowInstruction : Instruction {
        protected Instruction exitInstruction;
        public Instruction GetExitInstruction() {
            return exitInstruction;
        }
        public void SetExitInstruction(Instruction iIn) {
            exitInstruction = iIn;
        }

    }
}
