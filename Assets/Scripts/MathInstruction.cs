using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class MathInstruction : Instruction {
        protected float leftNum, rightNum;

        public abstract string GetMathSymbol();

        public MathInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
            if (GetArgument(IARG.LeftNumber) != null && GetArgument(IARG.RightNumber) != null) {
                leftNum = (float)Convert.ChangeType(GetArgument(IARG.LeftNumber).EvaluateArgument().GetValue(), typeof(float));
                rightNum = (float)Convert.ChangeType(GetArgument(IARG.RightNumber).EvaluateArgument().GetValue(), typeof(float));
            }
        }

        public override string ToString() {
            return GetMathSymbol();
        }


        public override void SetUpArgCompatabilityDict() {
            argCompatabilityDict = new Dictionary<IARG, HashSet<Type>> {
                { IARG.LeftNumber, new HashSet<Type> {  typeof(INumberDataType), typeof(MathInstruction) }  },
                { IARG.RightNumber, new HashSet<Type> {  typeof(INumberDataType), typeof(MathInstruction) }  }
            };
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", GetArgument(IARG.LeftNumber)?.DescriptiveInstructionToString(), " ", GetMathSymbol(), " ", GetArgument(IARG.RightNumber)?.DescriptiveInstructionToString());
        }
    }
}
