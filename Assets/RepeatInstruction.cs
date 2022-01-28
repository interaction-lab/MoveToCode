using System;

namespace MoveToCode {
    public class RepeatInstruction : SingleControlFlowInstruction {
        int output = 0, curIter = 0;
        public RepeatInstruction(CodeBlock cbIn) : base(cbIn) { }


        public override void EvaluateArgumentsOfInstruction() {
            output = (int)Convert.ChangeType(GetArgument(CommonSCKeys.RightNumber).EvaluateArgument().GetValue(), typeof(int));
            conditionIsTrue = curIter < output;
        }

        public override InstructionReturnValue RunInstruction() {
            if (!nextInstructionAddedToStack) {
                Interpreter.instance.AddToInstructionStack(GetNextInstruction());
                nextInstructionAddedToStack = true;
            }
            EvaluateArgumentsOfInstruction();
            if (conditionIsTrue) {
                Interpreter.instance.AddToInstructionStack(this);
                ++curIter;
                return new InstructionReturnValue(null, GetNestedInstruction());
            }
            return null; // done with loop
        }

        public override void ResestInternalState() {
            curIter = 0;
        }

        public override string ToString() {
            return "Repeat ";
        }
        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>", ToString(), "</color>", GetArgument(CommonSCKeys.RightNumber)?.DescriptiveInstructionToString(), ": ", GetNestedInstructionsAsString());
        }
    }
}