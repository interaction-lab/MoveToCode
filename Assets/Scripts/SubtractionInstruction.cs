namespace MoveToCode {
    public class SubtractionInstruction : Instruction {

        int leftInt, rightInt, numArguments;

        public SubtractionInstruction() {
            ResizeArgumentList(numArguments);
        }

        public SubtractionInstruction(IntDataType intLeft, IntDataType intRight) {
            SetArgumentAt(intLeft);
            SetArgumentAt(intRight);
        }

        public override void EvaluateArgumentList() {
            leftInt = argumentList[0].EvaluateArgument().GetValue();
            rightInt = argumentList[1].EvaluateArgument().GetValue();
        }

        public override int GetNumArguments() {
            return numArguments;
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            return new InstructionReturnValue(new IntDataType(leftInt - rightInt), GetNextInstruction());
        }
    }
}