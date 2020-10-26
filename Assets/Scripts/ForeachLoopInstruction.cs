using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class ForeachLoopInstruction : ControlFlowInstruction {
        private int currIdxInArray;

        public ForeachLoopInstruction(CodeBlock cbIn) : base(cbIn) {
            currIdxInArray = 0;
        }

        public override InstructionReturnValue RunInstruction() {
            if (!nextInstructionAddedToStack) {
                Interpreter.instance.AddToInstructionStack(GetNextInstruction());
                nextInstructionAddedToStack = true;
            }
            EvaluateArgumentsOfInstruction();
            if (conditionIsTrue) {
                currIdxInArray++;
                Interpreter.instance.AddToInstructionStack(this);
                return new InstructionReturnValue(null, GetNestedInstruction());
            }
            currIdxInArray = 0;
            return null;
        }

        public override void EvaluateArgumentsOfInstruction() {
            Variable iteratorVar = GetArgument(CommonSCKeys.Variable) as Variable;
            ArrayDataStructure dataStructureVar = GetArgument(CommonSCKeys.Array) as ArrayDataStructure;
            if (dataStructureVar != null && currIdxInArray < dataStructureVar.GetSize()) {
                iteratorVar.SetValue(dataStructureVar.GetValueAtIndex(currIdxInArray));
            }
        }

        public override string ToString() {
            return "foreach \nvar ";
        }

        public override string DescriptiveInstructionToString() {
            return "foreach still under development";
        }

    }
}