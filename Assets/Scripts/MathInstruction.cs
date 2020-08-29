using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class MathInstruction : Instruction {
        protected float leftNum, rightNum;

        public abstract string GetMathSymbol();

        public MathInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
            if (GetArgument(SNAPCOLTYPEDESCRIPTION.LeftNumber) != null && GetArgument(SNAPCOLTYPEDESCRIPTION.RightNumber) != null) {
                leftNum = (float)Convert.ChangeType(GetArgument(SNAPCOLTYPEDESCRIPTION.LeftNumber).EvaluateArgument().GetValue(), typeof(float));
                rightNum = (float)Convert.ChangeType(GetArgument(SNAPCOLTYPEDESCRIPTION.RightNumber).EvaluateArgument().GetValue(), typeof(float));
            }
        }

        public override string ToString() {
            return GetMathSymbol();
        }


        public override void SetUpArgToSnapColliderDict() {
            argToSnapColliderDict = new Dictionary<SNAPCOLTYPEDESCRIPTION, HashSet<Type>> {
                { SNAPCOLTYPEDESCRIPTION.LeftNumber, new HashSet<Type> {  typeof(INumberDataType), typeof(MathInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.RightNumber, new HashSet<Type> {  typeof(INumberDataType), typeof(MathInstruction) }  }
            };
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", GetArgument(SNAPCOLTYPEDESCRIPTION.LeftNumber)?.DescriptiveInstructionToString(), " ", GetMathSymbol(), " ", GetArgument(SNAPCOLTYPEDESCRIPTION.RightNumber)?.DescriptiveInstructionToString());
        }
    }
}
