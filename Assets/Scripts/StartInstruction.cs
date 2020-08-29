using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class StartInstruction : StandAloneInstruction {
        public static string startString = "Code Start";

        public StartInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
        }

        public override InstructionReturnValue RunInstruction() {
            ConsoleManager.instance.AddLine(startString);
            return new InstructionReturnValue(null, GetArgument(SNAPCOLTYPEDESCRIPTION.Next) as StandAloneInstruction);
        }

        public override string ToString() {
            return startString;
        }


        public override void SetUpArgToSnapColliderDict() {
            argToSnapColliderDict = new Dictionary<SNAPCOLTYPEDESCRIPTION, HashSet<Type>> {
               { SNAPCOLTYPEDESCRIPTION.Next, new HashSet<Type> { typeof(StandAloneInstruction) }  }
            };
        }

        public override string DescriptiveInstructionToString() {
            return ToString();
        }
    }
}