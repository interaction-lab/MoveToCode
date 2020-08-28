namespace MoveToCode {
    public abstract class ControlFlowInstruction : StandAloneInstruction {
        protected bool conditionIsTrue;
        protected bool exitInstructionAddedToStack;

        public ControlFlowInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void ResestInternalState() {
            exitInstructionAddedToStack = false;
        }

        public override void EvaluateArgumentList() {
            IDataType d = (GetArgument(IARG.Conditional) as ConditionalInstruction)?.RunInstruction().GetReturnDataVal();
            if (d != null) {
                conditionIsTrue = (bool)d.GetValue();
            }
        }

        protected string GetNestedInstructionsAsString() {
            string result = "";
            StandAloneInstruction currInstruction = GetNestedInstruction();
            while (currInstruction != null) {
                result = AddNestedInstructionTabbing(result, currInstruction);
                currInstruction = currInstruction.GetNextInstruction();
            }
            return result;
        }

        protected StandAloneInstruction GetNestedInstruction() {
            return GetArgument(IARG.Nested) as StandAloneInstruction;
        }

        private string AddNestedInstructionTabbing(string result, Instruction currInstruction) {
            return string.Join("",
                result,
                "\n    ",
                currInstruction.DescriptiveInstructionToString()
                    .Replace("\n    ", "\n        "));
        }

        public override StandAloneInstruction GetNextInstruction() {
            return GetArgument(IARG.Next) as StandAloneInstruction;
        }
    }
}
