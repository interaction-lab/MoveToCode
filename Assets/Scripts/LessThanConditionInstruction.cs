namespace MoveToCode {
    public class LessThanConditionInstruction : ConditionalInstruction {

        public LessThanConditionInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            return new InstructionReturnValue(new BoolDataType(null, (int)leftArg.GetValue() < (int)rightArg.GetValue()), null);
        }

        public override string GetConditionalSymbol() {
            return "<";
        }

        public override string GetCodeString() {
            return "<";
        }
    }
}