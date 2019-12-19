namespace MoveToCode {
    public abstract class MathInstruction : Instruction {
        protected float leftNum, rightNum;
        protected int numArguments = 2;

        public MathInstruction() {
            ResizeArgumentList(numArguments);
        }

        public MathInstruction(INumberDataType numLeftIn, INumberDataType numRightIn) {
            SetArgumentAt(numLeftIn);
            SetArgumentAt(numRightIn);
        }

        public override void EvaluateArgumentList() {
            leftNum = argumentList[0].EvaluateArgument().GetValue();
            rightNum = argumentList[1].EvaluateArgument().GetValue();
        }

        public override int GetNumArguments() {
            return numArguments;
        }

        public override void SetUpArgumentCompatability() {
            compatibileArgumentTypes.Add(typeof(INumberDataType));
        }
    }
}
