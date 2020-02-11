namespace MoveToCode {
    public class IfInstruction : SingleControlFlowInstruction {


        public override InstructionReturnValue RunInstruction() {
            Interpreter.instance.AddToInstructionStack(GetExitInstruction());
            EvaluateArgumentList();
            if (conditionIsTrue) {
                return new InstructionReturnValue(null, GetNextInstruction());
            }
            return null; // should go to endif
        }

        public override string ToString() {
            return "If: ";
        }
    }
}