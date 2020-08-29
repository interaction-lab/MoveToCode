using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class ForeachLoopInstruction : ControlFlowInstruction {
        private int currIdxInArray;

        public ForeachLoopInstruction(CodeBlock cbIn) : base(cbIn) {
            currIdxInArray = 0;
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
            Variable iteratorVar = GetArgument(SNAPCOLTYPEDESCRIPTION.Variable) as Variable;
            ArrayDataStructure dataStructureVar = GetArgument(SNAPCOLTYPEDESCRIPTION.ArrayDataStructure) as ArrayDataStructure;
            if (dataStructureVar != null && currIdxInArray < dataStructureVar.GetSize()) {
                iteratorVar.SetValue(dataStructureVar.GetValueAtIndex(currIdxInArray));
            }
        }


        public override void SetUpArgToSnapColliderDict() {
            argToSnapColliderDict = new Dictionary<SNAPCOLTYPEDESCRIPTION, HashSet<Type>> {
                { SNAPCOLTYPEDESCRIPTION.Nested, new HashSet<Type> { typeof(StandAloneInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.Variable, new HashSet<Type> {  typeof(Variable) }  },
                { SNAPCOLTYPEDESCRIPTION.ArrayDataStructure, new HashSet<Type> {  typeof(ArrayDataStructure) }  },
                { SNAPCOLTYPEDESCRIPTION.Next, new HashSet<Type> { typeof(StandAloneInstruction) }  }
            };
        }


        public override string ToString() {
            return "foreach \nvar ";
        }

    }
}