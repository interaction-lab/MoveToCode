using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class ArrayIndexInstruction : Instruction {
        protected IDataType leftArg, rightArg;

        public string GetIndexSymbol() {
            return "[]";
        }

        public ArrayIndexInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
            leftArg = GetArgumentAt(0)?.EvaluateArgument();
            rightArg = GetArgumentAt(1)?.EvaluateArgument();
        }

        public override int GetNumArguments() {
            return 2;
        }

        public override string ToString() {
            return GetIndexSymbol();
        }

        public override void SetUpArgPosToCompatability() {
            argPosToCompatability = new List<List<Type>> {
                new List<Type> {
                    typeof(IDataType), typeof(MathInstruction)
                },
                new List<Type> {
                    typeof(IDataType), typeof(MathInstruction)
                }
            };
        }

        public override void SetUpArgDescriptionList() {
            argDescriptionList = new List<string> { "Left side of condtional", "Right Side of Conditional" };
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", GetArgumentAt(0)?.DescriptiveInstructionToString(), " ", GetIndexSymbol(), " ", GetArgumentAt(1)?.DescriptiveInstructionToString());
        }
    }
}
