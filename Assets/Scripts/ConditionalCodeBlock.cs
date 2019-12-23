namespace MoveToCode {
    public class ConditionalCodeBlock : InstructionCodeBlock {
        public enum OPERATION {
            EQUALS,
            NOTEQUALS
        }

        public OPERATION op;

        protected override void SetInstructionOrData() {
            switch (op) {
                case OPERATION.EQUALS:
                    myInstruction = new EqualsConditionInstruction();
                    break;
                case OPERATION.NOTEQUALS:
                    myInstruction = new NotEqualsConditionInstruction();
                    break;
            }

        }
    }
}