namespace MoveToCode {
    public abstract class ControlFlowInstruction : StandAloneInstruction {
        protected bool conditionIsTrue;
        protected bool exitInstructionAddedToStack;

        public ControlFlowInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void ResestInternalState() {
            exitInstructionAddedToStack = false;
        }

        public override void EvaluateArgumentList() {
            IDataType d = (GetArgument(SNAPCOLTYPEDESCRIPTION.Conditional) as ConditionalInstruction)?.RunInstruction().GetReturnDataVal();
            if (d != null) {
                conditionIsTrue = (bool)d.GetValue();
            }
        }

        protected string GetNestedInstructionsAsString() {
            string result = "";
            for (StandAloneInstruction currInstruction = GetNestedInstruction(); currInstruction != null; currInstruction = currInstruction.GetNextInstruction()) {
                result = AddNestedInstructionTabbing(result, currInstruction);
            }
            return result;
        }

        protected StandAloneInstruction GetNestedInstruction() {
            return GetArgument(SNAPCOLTYPEDESCRIPTION.Nested) as StandAloneInstruction;
        }

        private string AddNestedInstructionTabbing(string result, Instruction currInstruction) {
            return string.Join("",
                result,
                "\n    ",
                currInstruction.DescriptiveInstructionToString()
                    .Replace("\n    ", "\n        "));
        }

        public override StandAloneInstruction GetNextInstruction() {
            return GetArgument(SNAPCOLTYPEDESCRIPTION.Next) as StandAloneInstruction;
        }
    }
}
