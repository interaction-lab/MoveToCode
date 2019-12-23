namespace MoveToCode {
    public abstract class MathInstruction : Instruction {
        protected float leftNum, rightNum;
        protected int numArguments = 2;

        public abstract string GetMathSymbol();

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

        public override string ToString() {
            return string.Join("", argumentList[0]?.ToString(), " ", GetMathSymbol(), " ", argumentList[1]?.ToString());
        }
    }
}
