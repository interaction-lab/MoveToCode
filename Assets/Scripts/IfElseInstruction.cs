namespace MoveToCode {
    public class IfElseInstruction : DoubleControlFlowInstruction {

        public override InstructionReturnValue RunInstruction() {
            Interpreter.instance.AddToInstructionStack(GetExitInstruction());
            EvaluateArgumentList();
            if (conditionIsTrue) {
                return new InstructionReturnValue(null, GetNextInstruction());
            }
            else {
                return new InstructionReturnValue(null, argumentList[1].GetCodeBlock().GetMyInstruction());
            }
        }

        public override string ToString() {
            return string.Join("", "If/Else: ", argumentList[0]?.ToString());
        }
    }
}