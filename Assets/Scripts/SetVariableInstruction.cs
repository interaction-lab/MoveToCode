using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class SetVariableInstruction : StandAloneInstruction {

        public SetVariableInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            if (GetArgument("Variable").GetType() == typeof(ArrayIndexInstruction)) {
                //set "variable"/element in array
                (GetArgument("Variable") as ArrayIndexInstruction).SetArrayValue(GetArgument("Value")?.EvaluateArgument());
                //set the array
                ((GetArgument("Variable") as ArrayIndexInstruction).GetArgument("Next") as Variable)
                    .SetValue((GetArgument("Variable") as ArrayIndexInstruction).GetArgument("Next").EvaluateArgument() as ArrayDataStructure);
            }
            else {
                //set regular variable (not in an array)
                (GetArgument("Variable") as Variable).SetValue(GetArgument("Value")?.EvaluateArgument());
            }
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            //return "Set\nVar"; // TODO: figure out this spacing
            return "";
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "Set ",
                GetArgument("Variable")?.DescriptiveInstructionToString(),
                " to ",
                GetArgument("Value")?.DescriptiveInstructionToString());
        }
    }
}