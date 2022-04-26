namespace MoveToCode {
    public class WhileInstruction : SingleControlFlowInstruction {

        public WhileInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override InstructionReturnValue RunInstruction() {
            if (!nextInstructionAddedToStack) {
                Interpreter.instance.AddToInstructionStack(GetNextInstruction());
                nextInstructionAddedToStack = true;
            }
            EvaluateArgumentsOfInstruction();
            if (conditionIsTrue) {
                Interpreter.instance.AddToInstructionStack(this);
                return new InstructionReturnValue(null, GetNestedInstruction());
            }
            return null; // done with loop
        }

        public override string ToString() {
            return "while ";
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