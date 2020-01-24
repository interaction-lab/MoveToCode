using UnityEngine;

namespace MoveToCode {
    public class PrintInstruction : Instruction {

        string output;

        public PrintInstruction() {
            ResizeArgumentList(GetNumArguments());
        }

        public PrintInstruction(IArgument argIn) {
            ResizeArgumentList(GetNumArguments());
            SetArgumentAt(argIn, 0);
        }

        public override void EvaluateArgumentList() {
            output = argumentList[0]?.EvaluateArgument()?.GetValue()?.ToString();
        }

        public override int GetNumArguments() {
            return 1;
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            Debug.Log(output);
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override void SetUpArgumentCompatability() {
            compatibileArgumentTypes.Add(typeof(IDataType));
            compatibileArgumentTypes.Add(typeof(Instruction));
        }

        public override string ToString() {
            return string.Join("", "Print: ", argumentList[0]?.ToString());
        }
    }
}