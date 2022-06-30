
namespace MoveToCode {
    public class PrintInstruction : SnappableStandAloneInstruction {
        string output;

        public PrintInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
            output = GetArgument(CommonSCKeys.Printable)?.EvaluateArgument()?.ToString();
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            ConsoleManager.instance.AddLine(output);
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            return "print";
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>" + ToString() + "</color>(", GetArgument(CommonSCKeys.Printable)?.DescriptiveInstructionToString() + ")");
        }

        public override string ToJSON() {
            return string.Join(",", new string[] {
                "{\"name\": \"" + ToString() + "\"",
                "\"type\": \"" + GetType().ToString() + "\"",
                "\"args\":{\"next\": " + GetArgumentJSON(CommonSCKeys.Next),
                "\"print\": " + GetArgumentJSON(CommonSCKeys.Printable) + "}}"
            });
        }
    }
}