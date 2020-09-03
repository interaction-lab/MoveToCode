namespace MoveToCode {
    public class EqualsConditionInstruction : ConditionalInstruction {

        public EqualsConditionInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            return new InstructionReturnValue(new BoolDataType(null, leftArg.IsSameDataTypeAndEqualTo(rightArg)), null);
        }

        public override string GetConditionalSymbol() {
            return "= TO";
        }

        public override string GetCodeString() {
            return "==";
        }
    }
}