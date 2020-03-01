namespace MoveToCode {
    public class InstructionReturnValue {
        IDataType returnDataVal;
        Instruction nextInstruction;

        public InstructionReturnValue(IDataType dTIn, Instruction iIn) {
            returnDataVal = dTIn;
            nextInstruction = iIn;
        }

        public IDataType GetReturnDataVal() {
            return returnDataVal;
        }
        public void SetReturnDataVal(IDataType dTIn) {
            returnDataVal = dTIn;
        }
        public Instruction GetNextInstruction() {
            return nextInstruction;
        }
        public void SetNextInstruction(Instruction iIn) {
            nextInstruction = iIn;
        }
    }
}