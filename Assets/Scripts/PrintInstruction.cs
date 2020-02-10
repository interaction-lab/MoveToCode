using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class PrintInstruction : StandAloneInstruction {
        string output;

        public PrintInstruction(IArgument argIn) {
            SetArgumentAt(argIn, 1);
        }

        public override void EvaluateArgumentList() {
            output = argumentList[1]?.EvaluateArgument()?.GetValue()?.ToString();
        }

        public override int GetNumArguments() {
            return 2;
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            ConsoleManager.instance.AddLine(output);
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            return "Print: ";
        }

        public override List<Type> GetArgCompatibilityAtPos(int pos) {
            if (pos == 0) {
                return new List<Type> { typeof(StandAloneInstruction) };
            }
            else {
                return new List<Type> { typeof(IDataType) };
            }
        }

        public override List<string> GetArgListDescription() {
            return new List<string> { "NextInstruction", "Arg0 (Thing that is printed" };
        }
    }
}