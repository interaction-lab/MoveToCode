using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class ConditionalInstruction : Instruction {
        protected IDataType leftArg, rightArg;
        KeyValuePair<Type, int> leftArgKey = new KeyValuePair<Type, int>(typeof(SnapColliderLeftOfConditional), 0);
        KeyValuePair<Type, int> rightArgKey = new KeyValuePair<Type, int>(typeof(SnapColliderRightOfConditional), 0);

        public abstract string GetConditionalSymbol();
        public abstract string GetCodeString();

        public ConditionalInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
            leftArg = GetArgument(leftArgKey)?.EvaluateArgument();
            rightArg = GetArgument(rightArgKey)?.EvaluateArgument();
        }

        public override string ToString() {
            return GetConditionalSymbol();
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("",
                        GetArgument(leftArgKey)?.DescriptiveInstructionToString(),
                        " ",
                        GetCodeString(),
                        " ",
                        GetArgument(rightArgKey)?.DescriptiveInstructionToString());
        }
    }
}
