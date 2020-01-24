namespace MoveToCode {
    public abstract class ConditionalInstruction : Instruction {
        protected IDataType leftArg, rightArg;

        public abstract string GetConditionalSymbol();

        public ConditionalInstruction() {
            ResizeArgumentList(GetNumArguments());
        }

        public ConditionalInstruction(IDataType numLeftIn, IDataType numRightIn) {
            ResizeArgumentList(GetNumArguments());
            SetArgumentAt(numLeftIn, 0);
            SetArgumentAt(numRightIn, 1);
        }

        public override void EvaluateArgumentList() {
            // check same data type both sides, throw exception if not
            leftArg = argumentList[0].EvaluateArgument();
            rightArg = argumentList[1].EvaluateArgument();
        }

        public override int GetNumArguments() {
            return 2;
        }

        public override void SetUpArgumentCompatability() {
            compatibileArgumentTypes.Add(typeof(IDataType));
            compatibileArgumentTypes.Add(typeof(Instruction));
        }

        public override string ToString() {
            return string.Join("", argumentList[0]?.ToString(),
                " ",
                GetConditionalSymbol(),
                " ",
                argumentList[1]?.ToString());
        }
    }
}
