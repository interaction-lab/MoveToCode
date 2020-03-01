namespace MoveToCode {
    public class SetVariableCodeBlock : StandAloneInstructionCodeBlock {
        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new SetVariableInstruction(this);
        }
    }
}