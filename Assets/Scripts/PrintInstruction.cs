using UnityEngine;

namespace MoveToCode {
    public class PrintInstruction : Instruction {

        string output;
        int numArguments = 1;

        public PrintInstruction() {
            ResizeArgumentList(numArguments);
        }

        public PrintInstruction(IArgument argIn) {
            SetArgumentAt(argIn, 0);
        }

        public override void EvaluateArgumentList() {
            output = argumentList[0].EvaluateArgument().GetValue().ToString();
        }

        public override int GetNumArguments() {
            return numArguments;
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            Debug.Log(output);
            return new InstructionReturnValue(null, GetNextInstruction());
        }
    }
}