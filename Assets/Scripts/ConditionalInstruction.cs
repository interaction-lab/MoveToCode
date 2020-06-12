using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class ConditionalInstruction : Instruction {
        protected IDataType leftArg, rightArg;

        public abstract string GetConditionalSymbol();

        public ConditionalInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
            leftArg = GetArgumentAt(0)?.EvaluateArgument();
            rightArg = GetArgumentAt(1)?.EvaluateArgument();
        }

        public override int GetNumArguments() {
            return 2;
        }

        public override string ToString() {
            return GetConditionalSymbol();
        }

        public override void SetUpArgPosToCompatability() {
            argPosToCompatability = new List<List<Type>> {
                new List<Type> {
                    typeof(BasicDataType), typeof(MathInstruction)
                },
                new List<Type> {
                    typeof(BasicDataType), typeof(MathInstruction)
                }
            };
        }

        public override void SetUpArgDescriptionList() {
            argDescriptionList = new List<string> { "Left side of condtional", "Right Side of Conditional" };
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", GetArgumentAt(0)?.DescriptiveInstructionToString(), " ", GetConditionalSymbol(), " ", GetArgumentAt(1)?.DescriptiveInstructionToString());
        }
    }
}
