namespace MoveToCode {
    public class IfElseInstruction : DoubleControlFlowInstruction {

        public IfElseInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override InstructionReturnValue RunInstruction() {
            Interpreter.instance.AddToInstructionStack(GetNextInstruction());
            EvaluateArgumentsOfInstruction();
            if (conditionIsTrue) {
                return new InstructionReturnValue(null, GetNestedInstruction());
            }
            else {
                return new InstructionReturnValue(null, (GetArgument("Next") as StandAloneInstruction));
            }
        }

        public override string ToString() {
            return "If/Else: ";
        }

        public override string DescriptiveInstructionToString() {
            return ToString(); // TODO fix one day
        }
    }
}