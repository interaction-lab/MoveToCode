﻿using System.Collections.Generic;

namespace MoveToCode {
    public class StartInstruction : StandAloneInstruction {
        public static string startString = "Code Start";

        public StartInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
        }

        public override InstructionReturnValue RunInstruction() {
            ConsoleManager.instance.AddLine(startString);
            return new InstructionReturnValue(null, GetArgument(
                new KeyValuePair<System.Type, int>(typeof(SnapColliderNext), 0)) as StandAloneInstruction);
        }

        public override string ToString() {
            return startString;
        }

        public override string DescriptiveInstructionToString() {
            return ToString();
        }
    }
}