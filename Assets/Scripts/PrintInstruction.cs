using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class PrintInstruction : StandAloneInstruction {
        string output;

        public PrintInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
            output = GetArgument(IARG.Printable)?.EvaluateArgument()?.ToString();
        }

        public override int GetNumArguments() {
            return 2;
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            ConsoleManager.instance.AddLine(output);
            return new InstructionReturnValue(null, GetNextInstruction());
        }


        public override void SetUpArgCompatabilityDict() {
            argCompatabilityDict = new Dictionary<IARG, HashSet<Type>> {
                { IARG.Next, new HashSet<Type> { typeof(StandAloneInstruction) }  },
                { IARG.Printable, new HashSet<Type> { typeof(IDataType), typeof(MathInstruction), typeof(ConditionalInstruction), typeof(ArrayIndexInstruction) }  }
            };
        }

        public override string ToString() {
            return "print";
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>" + ToString() + "</color>(", GetArgument(IARG.Printable)?.DescriptiveInstructionToString() + ")");
        }
    }
}