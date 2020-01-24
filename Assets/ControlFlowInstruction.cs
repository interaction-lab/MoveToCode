namespace MoveToCode {
    public abstract class ControlFlowInstruction : Instruction {
        protected Instruction exitInstruction;
        public Instruction GetExitInstruction() {
            return exitInstruction;
        }
        public void SetExitInstruction(Instruction iIn) {
            exitInstruction = iIn;
        }

        public int FindExitChainSize() {
            Instruction runner = GetExitInstruction();
            if (runner != null) {
                return runner.GetCodeBlock().FindChainSize() + 1;
            }
            return 0;
        }

    }
}
