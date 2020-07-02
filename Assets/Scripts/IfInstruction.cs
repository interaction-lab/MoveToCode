namespace MoveToCode {
    public class IfInstruction : SingleControlFlowInstruction {

        public IfInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override InstructionReturnValue RunInstruction() {
            Interpreter.instance.AddToInstructionStack(GetNextInstruction()); //changed Exit to Next
            EvaluateArgumentList();
            if (conditionIsTrue) {
                return new InstructionReturnValue(null, GetNestedInstruction()); //changed Next to Nested
            }
            return null; // should go to endif
        }

        public override string ToString() {
            return "if ";
        }
    }
}