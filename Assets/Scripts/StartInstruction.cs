using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class StartInstruction : Instruction {

        public StartInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
        }

        public override int GetNumArguments() {
            return 1;
        }

        public override InstructionReturnValue RunInstruction() {
            Debug.Log("Code START");
            return new InstructionReturnValue(null, GetArgumentAt(0) as StandAloneInstruction);
        }

        public override string ToString() {
            return "START";
        }

        public override void SetUpArgPosToCompatability() {
            argPosToCompatability = new List<List<Type>> {
                new List<Type>{
                    typeof(StandAloneInstruction)
                }
            };
        }

        public override void SetUpArgDescriptionList() {
            argDescriptionList = new List<string> { "NextInstruction" };
        }
    }
}