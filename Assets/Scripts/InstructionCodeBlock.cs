namespace MoveToCode {
    public abstract class InstructionCodeBlock : CodeBlock {
        public override string ToString() {
            return myInstruction.ToString();
        }
        public override bool IsDataCodeBlock() {
            return false;
        }
        public override bool IsInstructionCodeBlock() {
            return true;
        }
    }
}