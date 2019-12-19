using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoveToCode {
    public abstract class ConditionalInstruction : Instruction {
        int numArgs = 2;
        protected IDataType leftArg, rightArg;
        public override void EvaluateArgumentList() {
            // check same data type both sides, throw exception if not
            leftArg = argumentList[0].EvaluateArgument().GetValue();
            rightArg = argumentList[1].EvaluateArgument().GetValue();
        }

        public override int GetNumArguments() {
            return numArgs;
        }
    }
}
