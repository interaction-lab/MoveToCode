using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class ConditionalInstruction : Instruction {
        protected IDataType leftArg, rightArg;

        public abstract string GetConditionalSymbol();

        public override void EvaluateArgumentList() {
            leftArg = argumentList[0]?.EvaluateArgument();
            rightArg = argumentList[1]?.EvaluateArgument();
        }

        public override int GetNumArguments() {
            return 2;
        }

        public override string ToString() {
            return string.Join("", argumentList[0]?.ToString(),
                " ",
                GetConditionalSymbol(),
                " ",
                argumentList[1]?.ToString());
        }

        public override List<Type> GetArgCompatibilityAtPos(int pos) {
            return new List<Type> { typeof(IDataType), typeof(MathInstruction) };
        }
    }
}
