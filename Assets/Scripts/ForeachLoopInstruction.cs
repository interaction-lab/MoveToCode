using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class ForeachLoopInstruction : ControlFlowInstruction {
        private int currIdxInArray;

        public ForeachLoopInstruction(CodeBlock cbIn) : base(cbIn) {
            currIdxInArray = 0;
        }

        public override int GetNumArguments() {
            return 4;
        }

        public override InstructionReturnValue RunInstruction() {
            if (!exitInstructionAddedToStack) {
                Interpreter.instance.AddToInstructionStack(GetNextInstruction());
                exitInstructionAddedToStack = true;
            }
            EvaluateArgumentList();
            if (conditionIsTrue) {
                currIdxInArray++;
                Interpreter.instance.AddToInstructionStack(this);
                return new InstructionReturnValue(null, GetNestedInstruction());
            }
            currIdxInArray = 0;
            return null;
        }

        public override void EvaluateArgumentList() {
            Variable iteratorVar = GetArgument(IARG.Variable) as Variable;
            ArrayDataStructure dataStructureVar = GetArgument(IARG.ArrayDataStructure) as ArrayDataStructure;
            if (dataStructureVar != null && currIdxInArray < dataStructureVar.GetSize()) {
                iteratorVar.SetValue(dataStructureVar.GetValueAtIndex(currIdxInArray));
            }
        }


        public override void SetUpArgCompatabilityDict() {
            argCompatabilityDict = new Dictionary<IARG, HashSet<Type>> {
                { IARG.Nested, new HashSet<Type> { typeof(StandAloneInstruction) }  },
                { IARG.Variable, new HashSet<Type> {  typeof(Variable) }  },
                { IARG.ArrayDataStructure, new HashSet<Type> {  typeof(ArrayDataStructure) }  },
                { IARG.Next, new HashSet<Type> { typeof(StandAloneInstruction) }  }
            };
        }


        public override string ToString() {
            return "foreach \nvar ";
        }

    }
}