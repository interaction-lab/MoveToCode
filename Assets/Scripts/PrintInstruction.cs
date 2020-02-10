using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class PrintInstruction : StandAloneInstruction {
        string output;

        public PrintInstruction(CodeBlock cbIn) : base(cbIn) {

        }

        public override void EvaluateArgumentList() {
            output = GetArgumentAt(1)?.EvaluateArgument()?.GetValue()?.ToString();
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

        public override void SetUpArgPosToCompatability() {
            throw new NotImplementedException();
        }

        public override void SetUpArgDescriptionList() {
            throw new NotImplementedException();
        }

        public override void SetCodeBlock(CodeBlock codeBlock) {
            throw new NotImplementedException();
        }
    }
}