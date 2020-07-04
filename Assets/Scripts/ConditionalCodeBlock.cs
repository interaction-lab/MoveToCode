namespace MoveToCode {
    public class ConditionalCodeBlock : InstructionCodeBlock {
        public enum OPERATION {
            EQUALS,
            NOTEQUALS
        }

        public OPERATION op;

        protected override void SetMyBlockInternalArg() {
            switch (op) {
                case OPERATION.EQUALS:
                    myBlockInternalArg = new EqualsConditionInstruction(this);
                    break;
                case OPERATION.NOTEQUALS:
                    myBlockInternalArg = new NotEqualsConditionInstruction(this);
                    break;
            }
        }

        public void SetOperation(OPERATION opIn) {
            op = opIn;
        }
    }
}