using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class DoubleControlFlowInstruction : ControlFlowInstruction {
        public DoubleControlFlowInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override int GetNumArguments() {
            return 4;
        }
        protected override StandAloneInstruction GetExitInstruction() {
            return GetArgumentAt(3) as StandAloneInstruction;
        }
    }
}
