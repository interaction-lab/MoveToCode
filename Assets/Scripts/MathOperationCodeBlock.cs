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
                    myBlockInternalArg = new AdditionInstruction(this);
                    break;
                case OPERATION.DIV:
                    myBlockInternalArg = new DivisionInstruction(this);
                    break;
                case OPERATION.MUL:
                    myBlockInternalArg = new MultiplicationInstruction(this);
                    break;
                case OPERATION.SUB:
                    myBlockInternalArg = new SubtractionInstruction(this);
                    break;
            }

        }
    }
}