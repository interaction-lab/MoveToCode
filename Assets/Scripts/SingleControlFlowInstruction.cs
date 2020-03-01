using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class SingleControlFlowInstruction : ControlFlowInstruction {
        public SingleControlFlowInstruction(CodeBlock cbIn) : base(cbIn) { }
        public override int GetNumArguments() {
            return 3;
        }
        protected override StandAloneInstruction GetExitInstruction() {
            return GetArgumentAt(2) as StandAloneInstruction;
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", ToString(), GetArgumentAt(1)?.DescriptiveInstructionToString(), " then ", GetArgumentAt(0)?.DescriptiveInstructionToString());
        }
    }
}
