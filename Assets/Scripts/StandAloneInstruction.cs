namespace MoveToCode {
    public abstract class StandAloneInstruction : Instruction {

        public StandAloneInstruction(CodeBlock cbIn) : base(cbIn) { }

        public virtual StandAloneInstruction GetNextInstruction() {
            return GetArgument("Next") as StandAloneInstruction;
        }
    }
}
