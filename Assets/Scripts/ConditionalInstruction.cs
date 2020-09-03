namespace MoveToCode {
    public abstract class ConditionalInstruction : Instruction {
        protected IDataType leftArg, rightArg;

        public abstract string GetConditionalSymbol();
        public abstract string GetCodeString();

        public ConditionalInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
            leftArg = GetArgument(SNAPCOLTYPEDESCRIPTION.LeftOfConditional)?.EvaluateArgument();
            rightArg = GetArgument(SNAPCOLTYPEDESCRIPTION.RightOfConditional)?.EvaluateArgument();
        }

        public override string ToString() {
            return GetConditionalSymbol();
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("",
                        GetArgument(SNAPCOLTYPEDESCRIPTION.LeftOfConditional)?.DescriptiveInstructionToString(),
                        " ",
                        GetCodeString(),
                        " ",
                        GetArgument(SNAPCOLTYPEDESCRIPTION.RightOfConditional)?.DescriptiveInstructionToString());
        }
    }
}
