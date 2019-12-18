using UnityEngine;

namespace MoveToCode {
    public class PrintInstruction : Instruction {

        string output;

        public PrintInstruction() {
            ResizeArgumentList(1);
        }

        public PrintInstruction(IArgument argIn) {
            SetArgumentAt(argIn, 0);
        }

        public override void EvaluateArgumentList() {
            output = argumentList[0].EvaluateArgument().GetValue().ToString();
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            Debug.Log(output);
            return new InstructionReturnValue(null, GetNextInstruction());
        }
    }
}