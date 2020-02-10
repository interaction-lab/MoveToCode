namespace MoveToCode {
    public abstract class InstructionCodeBlock : CodeBlock {
        public StandAloneInstruction GetNextInstruction() {
            return (myBlockInternalArg as StandAloneInstruction).GetNextInstruction();
        }

        public int FindChainSize() {
            return FindChainSize(this);
        }

        // get all chain args should be control flow override
        public int FindChainSizeOfArgIndex(int indexIn) {
            return FindChainSize(codeBlockArgumentList[indexIn]) + GetBlockVerticalSize();
        }

        public int FindChainSize(CodeBlock cbIn) {
            if (cbIn == null) {
                return 0;
            }
            int size = 0;
            Instruction runner = cbIn as Instr // .GetMyInstruction().GetNextInstruction();
            while (runner != null) {
                size += runner.GetCodeBlock().GetBlockVerticalSize();
                ControlFlowInstruction cfi = runner as ControlFlowInstruction;
                if (cfi != null) { // this is to deal with big chains of control flow blocks changing at once
                    size += runner.GetCodeBlock().FindChainSize();
                    runner = cfi.GetExitInstruction();
                }
                else {
                    runner = runner.GetNextInstruction();
                }
            }
            return size;
        }
    }
}