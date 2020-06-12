using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class PrintInstruction : StandAloneInstruction {
        string output;

        public PrintInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
            output = GetArgumentAt(1)?.EvaluateArgument()?.ToString();
        }

        public override int GetNumArguments() {
            return 2;
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            ConsoleManager.instance.AddLine(output);
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            return "Print: ";
        }

        public override void SetUpArgPosToCompatability() {
            argPosToCompatability = new List<List<Type>> {
                new List<Type>{
                    typeof(StandAloneInstruction)
                },
                new List<Type> {
                    typeof(BasicDataType), typeof(MathInstruction), typeof(ConditionalInstruction)
                }
            };
        }

        public override void SetUpArgDescriptionList() {
            argDescriptionList = new List<string> { "NextInstruction", "Thing that is printed" };
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", ToString(), GetArgumentAt(1)?.DescriptiveInstructionToString());
        }
    }
}