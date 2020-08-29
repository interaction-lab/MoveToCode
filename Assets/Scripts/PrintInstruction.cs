using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class PrintInstruction : StandAloneInstruction {
        string output;

        public PrintInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
            output = GetArgument(SNAPCOLTYPEDESCRIPTION.Printable)?.EvaluateArgument()?.ToString();
        }


        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            ConsoleManager.instance.AddLine(output);
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            return "print";
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>" + ToString() + "</color>(", GetArgument(SNAPCOLTYPEDESCRIPTION.Printable)?.DescriptiveInstructionToString() + ")");
        }
    }
}