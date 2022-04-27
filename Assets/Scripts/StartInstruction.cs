using System.Collections.Generic;

namespace MoveToCode {
    public class StartInstruction : StandAloneInstruction {
        public static string startString = "Robot Commands";

        public StartInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
        }

        public override InstructionReturnValue RunInstruction() {
            ConsoleManager.instance.AddLine(startString);
            return new InstructionReturnValue(null, GetArgument(
                new KeyValuePair<System.Type, int>(typeof(NextSnapCollider), 0)) as StandAloneInstruction);
        }

        public override string ToString() {
            return startString;
        }

        public override string DescriptiveInstructionToString() {
            return ToString();
        }

        public override string ToJSON() {
            return string.Join(",", new string[] {
                "{\"name\": \"" + ToString() + "\"",
                "\"type\": \"" + GetType().ToString() + "\"",
                "\"args\":{\"next\": " + GetArgumentJSON(CommonSCKeys.Next) + "}}"
            });
        }
    }
}