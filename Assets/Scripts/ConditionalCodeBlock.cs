namespace MoveToCode {
    public class ConditionalCodeBlock : InstructionCodeBlock {
        public enum OPERATION {
            EQUALS,
            NOTEQUALS,
            LESSTHAN,
            GREATERTHAN
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
                case OPERATION.LESSTHAN:
                    myBlockInternalArg = new LessThanConditionInstruction(this);
                    break;
                case OPERATION.GREATERTHAN:
                    myBlockInternalArg = new GreaterThanConditionInstruction(this);
                    break;
            }
        }

        public void SetOperation(OPERATION opIn) {
            op = opIn;
        }
    }
}