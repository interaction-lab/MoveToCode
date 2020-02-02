using UnityEngine;

namespace MoveToCode {
    public class StartInstruction : Instruction {
        public override void EvaluateArgumentList() {
        }

        public override int GetNumArguments() {
            return 0;
        }

        public override InstructionReturnValue RunInstruction() {
            Debug.Log("Code START");
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            return "START";
        }
    }
}