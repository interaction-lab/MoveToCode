namespace MoveToCode {
    public abstract class MathInstruction : Instruction {
        protected float leftNum, rightNum;

        public abstract string GetMathSymbol();

        public override void EvaluateArgumentList() {
            leftNum = argumentList[0].EvaluateArgument().GetValue();
            rightNum = argumentList[1].EvaluateArgument().GetValue();
        }

        public override int GetNumArguments() {
            return 2;
        }

        public override string ToString() {
            return string.Join("", argumentList[0]?.ToString(), " ", GetMathSymbol(), " ", argumentList[1]?.ToString());
        }
    }
}
