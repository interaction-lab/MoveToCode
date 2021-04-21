namespace MoveToCode {
    public abstract class ControlFlowInstruction : SnappableStandAloneInstruction {
        protected bool conditionIsTrue;
        protected bool nextInstructionAddedToStack;

        public ControlFlowInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void ResestInternalState() {
            nextInstructionAddedToStack = false;
        }

        public override void EvaluateArgumentsOfInstruction() {
            IDataType d = (GetArgument(CommonSCKeys.Conditional) as ConditionalInstruction)?.RunInstruction().GetReturnDataVal();
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
            return GetArgument(CommonSCKeys.Nested) as StandAloneInstruction;
        }

        private string AddNestedInstructionTabbing(string result, Instruction currInstruction) {
            return string.Join("",
                result,
                "\n    ",
                currInstruction.DescriptiveInstructionToString()
                    .Replace("\n    ", "\n        "));
        }

        public override StandAloneInstruction GetNextInstruction() {
            return GetArgument(CommonSCKeys.Next) as StandAloneInstruction;
        }
    }
}
