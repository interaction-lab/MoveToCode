namespace MoveToCode {
    public class EqualsConditionInstruction : ConditionalInstruction {
        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            return new InstructionReturnValue(new BoolDataType(leftArg.IsSameDataTypeAndEqualTo(rightArg)), GetNextInstruction());
        }
    }
}