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
            return new InstructionReturnValue(null, GetArgument(IARG.Next) as StandAloneInstruction);
        }

        public override string ToString() {
            return startString;
        }

        //public override void SetUpArgPosToCompatability(){
        //    argPosToCompatability = new List<List<Type>> {
        //        new List<Type>{
        //            typeof(StandAloneInstruction)
        //        }
        //    };
        //}

        //public override void SetUpArgDescriptionList() { 
        //    argDescriptionList = new List<string> { "NextInstruction" };
        //}
        public override void SetUpArgCompatabilityDict() {
            argCompatabilityDict = new Dictionary<IARG, HashSet<Type>> {
               { IARG.Next, new HashSet<Type> { typeof(StandAloneInstruction) }  }
            };
        }

        public override string DescriptiveInstructionToString() {
            return ToString();
        }
    }
}