namespace MoveToCode {
    public class SubtractionInstruction : Instruction {

        int leftInt, rightInt;
        public SubtractionInstruction() {
            ResizeArgumentList(2);
        }

        public SubtractionInstruction(IntDataType intLeft, IntDataType intRight) {
            SetArgumentAt(intLeft);
            SetArgumentAt(intRight);
        }

        public override void EvaluateArgumentList() {
            leftInt = argumentList[0].EvaluateArgument().GetValue();
            rightInt = argumentList[1].EvaluateArgument().GetValue();
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            return new InstructionReturnValue(new IntDataType(leftInt - rightInt), GetNextInstruction());
        }
    }
}