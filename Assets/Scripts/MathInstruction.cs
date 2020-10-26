using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class MathInstruction : Instruction {
        protected float leftNum, rightNum;

        public abstract string GetMathSymbol();

        public MathInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
            if (GetArgument(CommonSCKeys.LeftNumber) != null && GetArgument(CommonSCKeys.RightNumber) != null) {
                leftNum = (float)Convert.ChangeType(GetArgument(CommonSCKeys.LeftNumber).EvaluateArgument().GetValue(), typeof(float));
                rightNum = (float)Convert.ChangeType(GetArgument(CommonSCKeys.RightNumber).EvaluateArgument().GetValue(), typeof(float));
            }
        }

        public override string ToString() {
            return GetMathSymbol();
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", GetArgument(CommonSCKeys.LeftNumber)?.DescriptiveInstructionToString(), " ", GetMathSymbol(), " ", GetArgument(CommonSCKeys.RightNumber)?.DescriptiveInstructionToString());
        }
    }
}
