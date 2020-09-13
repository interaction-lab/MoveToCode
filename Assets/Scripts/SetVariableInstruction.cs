using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class SetVariableInstruction : StandAloneInstruction {

        public SetVariableInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            if (GetArgument(SNAPCOLTYPEDESCRIPTION.Variable).GetType() == typeof(ArrayIndexInstruction)) {
                //set "variable"/element in array
                (GetArgument(SNAPCOLTYPEDESCRIPTION.Variable) as ArrayIndexInstruction).SetArrayValue(GetArgument(SNAPCOLTYPEDESCRIPTION.Value)?.EvaluateArgument());
                //set the array
                ((GetArgument(SNAPCOLTYPEDESCRIPTION.Variable) as ArrayIndexInstruction).GetArgument(SNAPCOLTYPEDESCRIPTION.Next) as Variable)
                    .SetValue((GetArgument(SNAPCOLTYPEDESCRIPTION.Variable) as ArrayIndexInstruction).GetArgument(SNAPCOLTYPEDESCRIPTION.Next).EvaluateArgument() as ArrayDataStructure);
            }
            else {
                //set regular variable (not in an array)
                (GetArgument(SNAPCOLTYPEDESCRIPTION.Variable) as Variable).SetValue(GetArgument(SNAPCOLTYPEDESCRIPTION.Value)?.EvaluateArgument());
            }
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            //return "Set\nVar"; // TODO: figure out this spacing
            return "";
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "Set ",
                GetArgument(SNAPCOLTYPEDESCRIPTION.Variable)?.DescriptiveInstructionToString(),
                " to ",
                GetArgument(SNAPCOLTYPEDESCRIPTION.Value)?.DescriptiveInstructionToString());
        }
    }
}