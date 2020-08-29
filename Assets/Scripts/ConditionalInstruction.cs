using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class ConditionalInstruction : Instruction {
        protected IDataType leftArg, rightArg;

        public abstract string GetConditionalSymbol();
        public abstract string GetCodeString();

        public ConditionalInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
            leftArg = GetArgument(IARG.LeftOfConditional)?.EvaluateArgument();
            rightArg = GetArgument(IARG.RightOfConditional)?.EvaluateArgument();
        }


        public override string ToString() {
            return GetConditionalSymbol();
        }

        public override void SetUpArgCompatabilityDict() {
            argCompatabilityDict = new Dictionary<IARG, HashSet<Type>> {
                { IARG.LeftOfConditional, new HashSet<Type> {  typeof(BasicDataType), typeof(MathInstruction), typeof(ArrayIndexInstruction) }  },
                { IARG.RightOfConditional, new HashSet<Type> {  typeof(BasicDataType), typeof(MathInstruction), typeof(ArrayIndexInstruction) }  }
            };
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", GetArgument(IARG.LeftOfConditional)?.DescriptiveInstructionToString(),
                        " ", GetCodeString(), " ", GetArgument(IARG.RightOfConditional)?.DescriptiveInstructionToString());
        }
    }
}
