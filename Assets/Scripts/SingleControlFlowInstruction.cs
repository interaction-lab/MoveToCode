using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class SingleControlFlowInstruction : ControlFlowInstruction {
        public SingleControlFlowInstruction(CodeBlock cbIn) : base(cbIn) { }
        public override int GetNumArguments() {
            return 3;
        }
        protected override StandAloneInstruction GetNestedInstruction() { 
            return GetArgumentAt(0) as StandAloneInstruction;
        }
        protected override string GetNestedInstructionsAsString(){
            string result = "";
            StandAloneInstruction currInstruction = GetNestedInstruction();
            while (currInstruction != null){
                result += "\n\t" + ReplaceFirst(currInstruction.DescriptiveInstructionToString(), "\t","\t\t"); //add nested instructions as tabbed, making sure tabs accumulate
                currInstruction = currInstruction.GetNextInstruction();
            }
            return result;

        }

        public override StandAloneInstruction GetNextInstruction(){ //overriding Next
            return GetArgumentAt(2) as StandAloneInstruction;
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", ToString(), GetArgumentAt(1)?.DescriptiveInstructionToString(), ": ", GetNestedInstructionsAsString());
        }
    }
}
