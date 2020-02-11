namespace MoveToCode {
    public class IfElseInstruction : DoubleControlFlowInstruction {

        public IfElseInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override InstructionReturnValue RunInstruction() {
            Interpreter.instance.AddToInstructionStack(GetExitInstruction());
            EvaluateArgumentList();
            if (conditionIsTrue) {
                return new InstructionReturnValue(null, GetNextInstruction());
            }
            else {
                return new InstructionReturnValue(null, (GetArgumentAt(2) as StandAloneInstruction));
            }
        }

        public override string ToString() {
            return "If/Else: ";
        }
    }
}