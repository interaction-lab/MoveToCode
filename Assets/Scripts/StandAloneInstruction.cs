namespace MoveToCode {
    public abstract class StandAloneInstruction : Instruction {
        public abstract StandAloneInstruction GetNextInstruction();

        public StandAloneInstruction(CodeBlock cbIn) : base(cbIn) { }
    }
}
