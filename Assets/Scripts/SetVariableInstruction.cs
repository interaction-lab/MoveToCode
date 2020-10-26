using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class SetVariableInstruction : StandAloneInstruction {

        public SetVariableInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            if (GetArgument(CommonSCKeys.Variable).GetType() == typeof(ArrayIndexInstruction)) {
                //set "variable"/element in array
                (GetArgument(CommonSCKeys.Variable) as ArrayIndexInstruction).SetArrayValue(GetArgument(CommonSCKeys.Value)?.EvaluateArgument());
                //set the array
                ((GetArgument(CommonSCKeys.Variable) as ArrayIndexInstruction).GetArgument(CommonSCKeys.Next) as Variable)
                    .SetValue((GetArgument(CommonSCKeys.Variable) as ArrayIndexInstruction).GetArgument(CommonSCKeys.Next).EvaluateArgument() as ArrayDataStructure);
            }
            else {
                //set regular variable (not in an array)
                (GetArgument(CommonSCKeys.Variable) as Variable).SetValue(GetArgument(CommonSCKeys.Value)?.EvaluateArgument());
            }
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            //return "Set\nVar"; // TODO: figure out this spacing
            return "";
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "Set ",
                GetArgument(CommonSCKeys.Variable)?.DescriptiveInstructionToString(),
                " to ",
                GetArgument(CommonSCKeys.Value)?.DescriptiveInstructionToString());
        }
    }
}