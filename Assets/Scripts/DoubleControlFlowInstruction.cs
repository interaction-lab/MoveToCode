using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class DoubleControlFlowInstruction : ControlFlowInstruction {
        public DoubleControlFlowInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override int GetNumArguments() {
            return 4;
        }
        public override StandAloneInstruction GetNextInstruction() {
            return GetArgumentAt(3) as StandAloneInstruction;
        }
        protected override StandAloneInstruction GetNestedInstruction() {
            return GetArgumentAt(0) as StandAloneInstruction;
        }
        
        protected override string GetNestedInstructionsAsString() {
            string result = "";
            StandAloneInstruction currInstruction = GetNestedInstruction();
            while (currInstruction != null)
            {
                result += "\n\t" + ReplaceFirst(currInstruction.DescriptiveInstructionToString(), "\t", "\t\t"); //add nested instructions as tabbed, making sure tabs accumulate
                currInstruction = currInstruction.GetNextInstruction();
            }
            return result;

        }
    }
}
