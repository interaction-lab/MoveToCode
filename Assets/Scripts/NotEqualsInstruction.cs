namespace MoveToCode {
    public class NotEqualsConditionInstruction : ConditionalInstruction {

        public NotEqualsConditionInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            return new InstructionReturnValue(new BoolDataType(!leftArg.IsSameDataTypeAndEqualTo(rightArg)), null);
        }

        public override string GetConditionalSymbol() {
            return "!=";
        }

    }
}