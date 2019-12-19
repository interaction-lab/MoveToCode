namespace MoveToCode {
    public abstract class ConditionalInstruction : Instruction {
        int numArgs = 2;
        protected IDataType leftArg, rightArg;
        public override void EvaluateArgumentList() {
            // check same data type both sides, throw exception if not
            leftArg = argumentList[0].EvaluateArgument();
            rightArg = argumentList[1].EvaluateArgument();
        }

        public override int GetNumArguments() {
            return numArgs;
        }

        public override void SetUpArgumentCompatability() {
            compatibileArgumentTypes.Add(typeof(IDataType));
            compatibileArgumentTypes.Add(typeof(Instruction));
        }
    }
}
