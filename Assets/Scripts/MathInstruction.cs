namespace MoveToCode {
    public abstract class MathInstruction : Instruction {
        protected int leftInt, rightInt, numArguments = 2;

        public MathInstruction() {
            ResizeArgumentList(numArguments);
        }

        public MathInstruction(IntDataType intLeft, IntDataType intRight) {
            SetArgumentAt(intLeft);
            SetArgumentAt(intRight);
        }

        public override void EvaluateArgumentList() {
            leftInt = argumentList[0].EvaluateArgument().GetValue();
            rightInt = argumentList[1].EvaluateArgument().GetValue();
        }

        public override int GetNumArguments() {
            return numArguments;
        }
    }
}
