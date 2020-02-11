using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class ConditionalInstruction : Instruction {
        protected IDataType leftArg, rightArg;

        public abstract string GetConditionalSymbol();

        public ConditionalInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
            leftArg = GetArgumentAt(1)?.EvaluateArgument();
            rightArg = GetArgumentAt(2)?.EvaluateArgument();
        }

        public override int GetNumArguments() {
            return 3;
        }

        public override string ToString() {
            return GetConditionalSymbol();
        }

        public override void SetUpArgPosToCompatability() {
            argPosToCompatability = new List<List<Type>> {
                new List<Type>{
                    typeof(StandAloneInstruction)
                },
                new List<Type> {
                    typeof(IDataType), typeof(MathInstruction)
                },
                new List<Type> {
                    typeof(IDataType), typeof(MathInstruction)
                }
            };
        }

        public override void SetUpArgDescriptionList() {
            argDescriptionList = new List<string> { "NextInstruction", "Left side of condtional", "Right Side of Conditional" };
        }
    }
}
