namespace MoveToCode {
    public class NotEqualsConditionInstruction : ConditionalInstruction {
        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            return new InstructionReturnValue(new BoolDataType(!leftArg.IsSameDataTypeAndEqualTo(rightArg)), GetNextInstruction());
        }

        public override string GetConditionalSymbol() {
            return "!=";
        }
    }
}