using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class PrintInstruction : StandAloneInstruction {
        string output;

        public PrintInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
            output = GetArgument(SNAPCOLTYPEDESCRIPTION.Printable)?.EvaluateArgument()?.ToString();
        }


        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            ConsoleManager.instance.AddLine(output);
            return new InstructionReturnValue(null, GetNextInstruction());
        }


        public override void SetUpArgToSnapColliderDict() {
            argToSnapColliderDict = new Dictionary<SNAPCOLTYPEDESCRIPTION, HashSet<Type>> {
                { SNAPCOLTYPEDESCRIPTION.Next, new HashSet<Type> { typeof(StandAloneInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.Printable, new HashSet<Type> { typeof(IDataType), typeof(MathInstruction), typeof(ConditionalInstruction), typeof(ArrayIndexInstruction) }  }
            };
        }

        public override string ToString() {
            return "print";
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>" + ToString() + "</color>(", GetArgument(SNAPCOLTYPEDESCRIPTION.Printable)?.DescriptiveInstructionToString() + ")");
        }
    }
}