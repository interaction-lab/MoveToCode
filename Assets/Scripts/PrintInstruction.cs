using UnityEngine;

namespace MoveToCode {
    public class PrintInstruction : Instruction {

        public PrintInstruction() {
            ResizeArgumentList(1);
        }

        public PrintInstruction(IArgument argIn) {
            AddArgument(argIn, 0);
        }


        public override InstructionReturnValue RunInstruction() {
            Debug.Log(argumentList[0].EvaluateArgument().GetValue());
            return new InstructionReturnValue(null, GetNextInstruction());
        }
    }
}