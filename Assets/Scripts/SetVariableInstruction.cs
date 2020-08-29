using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class SetVariableInstruction : StandAloneInstruction {

        public SetVariableInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
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
            return "Set\nVar"; // TODO: figure out this spacing
        }


        public override void SetUpArgToSnapColliderDict() {
            argToSnapColliderDict = new Dictionary<SNAPCOLTYPEDESCRIPTION, HashSet<Type>> {
                { SNAPCOLTYPEDESCRIPTION.Next, new HashSet<Type> { typeof(StandAloneInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.Variable, new HashSet<Type> { typeof(Variable) , typeof(ArrayIndexInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.Value, new HashSet<Type> { typeof(IDataType), typeof(MathInstruction), typeof(ConditionalInstruction), typeof(ArrayIndexInstruction) }  }
            };
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "Set ",
                GetArgument(SNAPCOLTYPEDESCRIPTION.Variable)?.DescriptiveInstructionToString(),
                " to ",
                GetArgument(SNAPCOLTYPEDESCRIPTION.Value)?.DescriptiveInstructionToString());
        }
    }
}