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
                SetCurIter(curIter + 1);
                MyCodeBlock.UpdateText();
                return new InstructionReturnValue(null, GetNestedInstruction());
            }
            return null; // done with loop
        }

        public override void ResestInternalState() {
            SetCurIter(0);
        }

        private void SetCurIter(int num) {
            curIter = num;
            MyCodeBlock.UpdateText();
        }

        public override string ToString() {
            return "Repeat " + curIter.ToString() + " of ";
        }
        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>", ToString(), "</color>", GetArgument(CommonSCKeys.RightNumber)?.DescriptiveInstructionToString(), ": ", GetNestedInstructionsAsString());
        }

        private void SetIterNum(int num) {
            ((GetArgument(CommonSCKeys.RightNumber) as INumberDataType)?.MyCodeBlock as IntCodeBlock)?.SetOutput(num);
        }
    }
}