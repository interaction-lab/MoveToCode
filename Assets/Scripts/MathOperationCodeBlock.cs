namespace MoveToCode {
    public class MathOperationCodeBlock : CodeBlock {
        public enum OPERATION {
            ADD,
            DIV,
            MUL,
            SUB
        }

        public OPERATION op;

        protected override void SetInstructionOrData() {
            switch (op) {
                case OPERATION.ADD:
                    myInstruction = new AdditionInstruction();
                    break;
                case OPERATION.DIV:
                    myInstruction = new DivisionInstruction();
                    break;
                case OPERATION.MUL:
                    myInstruction = new MultiplicationInstruction();
                    break;
                case OPERATION.SUB:
                    myInstruction = new SubtractionInstruction();
                    break;
            }

        }
    }
}