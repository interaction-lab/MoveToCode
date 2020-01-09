namespace MoveToCode {
    public class MathOperationCodeBlock : InstructionCodeBlock {
        public enum OPERATION {
            ADD,
            DIV,
            MUL,
            SUB
        }

        public OPERATION op;

        protected override void SetMyBlockInternalArg() {
            switch (op) {
                case OPERATION.ADD:
                    myBlockInternalArg = new AdditionInstruction();
                    break;
                case OPERATION.DIV:
                    myBlockInternalArg = new DivisionInstruction();
                    break;
                case OPERATION.MUL:
                    myBlockInternalArg = new MultiplicationInstruction();
                    break;
                case OPERATION.SUB:
                    myBlockInternalArg = new SubtractionInstruction();
                    break;
            }

        }
    }
}