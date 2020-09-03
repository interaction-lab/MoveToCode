using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class DoubleControlFlowInstruction : ControlFlowInstruction {
        public DoubleControlFlowInstruction(CodeBlock cbIn) : base(cbIn) { }
    }
}
