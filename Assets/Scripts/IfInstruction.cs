namespace MoveToCode {
    public class IfInstruction : SingleControlFlowInstruction {

        public IfInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override InstructionReturnValue RunInstruction() {
            Interpreter.instance.AddToInstructionStack(GetNextInstruction());
            EvaluateArgumentsOfInstruction();
            if (conditionIsTrue) {
                return new InstructionReturnValue(null, GetNestedInstruction());
            }
            return null; // should go to endif
        }

        public override string ToString() {
            return "if ";
        }

        public override string ToJSON() {
            return string.Join(",", new string[] {
                "{\"name\": \"" + ToString() + "\"",
                "\"type\": \"" + GetType().ToString() + "\"",
                "\"args\":{\"condition\": " + GetArgumentJSON(CommonSCKeys.Conditional),
                "\"nested\": " + GetArgumentJSON(CommonSCKeys.Nested),
                "\"next\": " + GetArgumentJSON(CommonSCKeys.Next) + "}}"
            });
        }
    }
}