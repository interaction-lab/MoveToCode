namespace MoveToCode {
    public class IfInstruction : ControlFlowInstruction {

        bool conditionIsTrue;

        public IfInstruction() {
            ResizeArgumentList(GetNumArguments());
        }

        public override void EvaluateArgumentList() {
            conditionIsTrue = (argumentList[0] as ConditionalInstruction)?.RunInstruction().GetReturnDataVal().GetValue();
        }

        public override InstructionReturnValue RunInstruction() {
            Interpreter.instance.AddToInstructionStack(GetExitInstruction());
            EvaluateArgumentList();
            if (conditionIsTrue) {
                return new InstructionReturnValue(null, GetNextInstruction());
            }
            return null; // should go to endif
        }


        public override void SetUpArgumentCompatability() {
            compatibileArgumentTypes.Add(typeof(ConditionalInstruction));
        }

        public override int GetNumArguments() {
            return 1;
        }

        public override string ToString() {
            return string.Join("", "If: ", argumentList[0]?.ToString());
        }
    }
}