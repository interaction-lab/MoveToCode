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
            if(GetArgument(IARG.Variable).GetType() == typeof(ArrayIndexInstruction)) {
                //set "variable"/element in array
                (GetArgument(IARG.Variable) as ArrayIndexInstruction).SetArrayValue(GetArgument(IARG.Value)?.EvaluateArgument());
                //set the array
                ((GetArgument(IARG.Variable) as ArrayIndexInstruction).GetArgument(IARG.Next) as Variable)
                    .SetValue((GetArgument(IARG.Variable) as ArrayIndexInstruction).GetArgument(IARG.Next).EvaluateArgument() as ArrayDataStructure);
            } else {
                //set regular variable (not in an array)
                (GetArgument(IARG.Variable) as Variable).SetValue(GetArgument(IARG.Value)?.EvaluateArgument());
            }
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            return "Set\nVar"; // TODO: figure out this spacing
        }

        //public override void SetUpArgPosToCompatability() {
        //    argPosToCompatability = new List<List<Type>> {
        //        new List<Type>{
        //            typeof(StandAloneInstruction)
        //        },
        //        new List<Type> {
        //            typeof(Variable), typeof(ArrayIndexInstruction)
        //        },
        //        new List<Type> {
        //            typeof(IDataType), typeof(MathInstruction), typeof(ConditionalInstruction), typeof(ArrayIndexInstruction)
        //        }
        //    };
        //}

        //// drop down of variables
        //public override void SetUpArgDescriptionList() {
        //    argDescriptionList = new List<string> { "NextInstruction", "Variable to be changed", "New variable value" };
        //}
        public override void SetUpArgCompatabilityDict() {
            argCompatabilityDict = new Dictionary<IARG, HashSet<Type>> {
                { IARG.Next, new HashSet<Type> { typeof(StandAloneInstruction) }  },
                { IARG.Variable, new HashSet<Type> { typeof(Variable) , typeof(ArrayIndexInstruction) }  },
                { IARG.Value, new HashSet<Type> { typeof(IDataType), typeof(MathInstruction), typeof(ConditionalInstruction), typeof(ArrayIndexInstruction) }  }
            };
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "Set ", GetArgument(IARG.Variable)?.DescriptiveInstructionToString(), " to ", GetArgument(IARG.Value)?.DescriptiveInstructionToString());
        }
    }
}