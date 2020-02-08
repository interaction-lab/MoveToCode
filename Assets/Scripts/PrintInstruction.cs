using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class PrintInstruction : StandAloneInstruction {
        string output;

        public PrintInstruction(IArgument argIn) {
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
            ConsoleManager.instance.AddLine(output);
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            return string.Join("", "Print: ", argumentList[0]?.ToString());
        }

        public override List<Type> GetArgCompatibilityAtPos(int pos) {
            return new List<Type> { typeof(IArgument) };
        }
    }
}