using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

        public override List<Type> GetArgCompatibilityAtPos(int pos) {
            Assert.IsTrue(false); // This should never have an arg
            return new List<Type> { };
        }
    }
}