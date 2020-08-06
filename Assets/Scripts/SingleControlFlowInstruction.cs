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
                result += "\n    " + currInstruction.DescriptiveInstructionToString().Replace("\n    ", "\n        "); //add nested instructions with accumulated tabbing
                currInstruction = currInstruction.GetNextInstruction();
            }
            return result;

        }

        public override StandAloneInstruction GetNextInstruction(){ 
            return GetArgumentAt(2) as StandAloneInstruction;
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>", ToString(), "</color>", GetArgumentAt(1)?.DescriptiveInstructionToString(), ": ", GetNestedInstructionsAsString());
        }
    }
}
