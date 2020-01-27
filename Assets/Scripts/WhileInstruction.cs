namespace MoveToCode {
    public class WhileInstruction : ControlFlowInstruction {

        bool conditionIsTrue;
        bool exitInstructionAddedToStack;

        public WhileInstruction() {
            ResizeArgumentList(GetNumArguments());
            exitInstructionAddedToStack = false;
        }

        public override void EvaluateArgumentList() {
            conditionIsTrue = (argumentList[0] as ConditionalInstruction)?.RunInstruction().GetReturnDataVal().GetValue();
        }

        public override InstructionReturnValue RunInstruction() {
            if (!exitInstructionAddedToStack) {
                Interpreter.instance.AddToInstructionStack(GetExitInstruction());
                exitInstructionAddedToStack = true;
            }
            EvaluateArgumentList();
            if (conditionIsTrue) {
                // put me on top of stack when while loop ends
                Interpreter.instance.AddToInstructionStack(this);
                return new InstructionReturnValue(null, GetNextInstruction());
            }
            return null; // done with loop
        }


        public override void SetUpArgumentCompatability() {
            compatibileArgumentTypes.Add(typeof(ConditionalInstruction));
        }

        public override int GetNumArguments() {
            return 1;
        }

        public override string ToString() {
            return string.Join("", "While: ", argumentList[0]?.ToString());
        }
    }
}