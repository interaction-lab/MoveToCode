using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class ConditionalInstruction : Instruction {
        protected IDataType leftArg, rightArg;

        public abstract string GetConditionalSymbol();
        public abstract string GetCodeString();

        public ConditionalInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
           // leftArg = GetArgumentAt(0)?.EvaluateArgument();
          //  rightArg = GetArgumentAt(1)?.EvaluateArgument();
            leftArg = GetArgument(IARG.LeftOfConditional)?.EvaluateArgument();
            rightArg = GetArgument(IARG.RightOfConditional)?.EvaluateArgument();
        }

        public override int GetNumArguments() {
            return 2;
        }

        public override string ToString() {
            return GetConditionalSymbol();
        }

        //public override void SetUpArgPosToCompatability() {
        //    argPosToCompatability = new List<List<Type>> {
        //        new List<Type> {
        //            typeof(BasicDataType), typeof(MathInstruction), typeof(ArrayIndexInstruction)
        //        },
        //        new List<Type> {
        //            typeof(BasicDataType), typeof(MathInstruction), typeof(ArrayIndexInstruction)
        //        }
        //    };
        //}

        //public override void SetUpArgDescriptionList() {
        //    argDescriptionList = new List<string> { "Left side of conditional", "Right Side of Conditional" };
        //}
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
