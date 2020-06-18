using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class SetVariableInstruction : StandAloneInstruction {

        public SetVariableInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
        }

        public override int GetNumArguments() {
            return 3;
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            (GetArgumentAt(1) as Variable).SetValue(GetArgumentAt(2)?.EvaluateArgument());
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            return "Set\nVar"; // TODO: figure out this spacing
        }

        public override void SetUpArgPosToCompatability() {
            argPosToCompatability = new List<List<Type>> {
                new List<Type>{
                    typeof(StandAloneInstruction)
                },
                new List<Type> {
                    typeof(Variable)
                },
                new List<Type> {
                    typeof(IDataType), typeof(MathInstruction), typeof(ConditionalInstruction), typeof(ArrayIndexInstruction)
                }
            };
        }

        // drop down of variables
        public override void SetUpArgDescriptionList() {
            argDescriptionList = new List<string> { "NextInstruction", "Variable to be changed", "New variable value" };
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "Set ", GetArgumentAt(1)?.DescriptiveInstructionToString(), " to ", GetArgumentAt(2)?.DescriptiveInstructionToString());
        }
    }
}