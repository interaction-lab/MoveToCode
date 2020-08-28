namespace MoveToCode {
    public abstract class StandAloneInstruction : Instruction {

        public StandAloneInstruction(CodeBlock cbIn) : base(cbIn) { }

        // Standard is that first instruction is next instruction
        // This might change when we chain arguments for flow
        public virtual StandAloneInstruction GetNextInstruction() {
            return GetArgument(IARG.Next) as StandAloneInstruction;
        }
    }
}
