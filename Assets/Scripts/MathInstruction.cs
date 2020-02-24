using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class MathInstruction : Instruction {
        protected float leftNum, rightNum;

        public abstract string GetMathSymbol();

        public MathInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
            if (GetArgumentAt(0) != null && GetArgumentAt(1) != null) {
                leftNum = (float)Convert.ChangeType(GetArgumentAt(0).EvaluateArgument().GetValue(), typeof(float));
                rightNum = (float)Convert.ChangeType(GetArgumentAt(1).EvaluateArgument().GetValue(), typeof(float));
            }
        }

        public override int GetNumArguments() {
            return 2;
        }

        public override string ToString() {
            return GetMathSymbol();
        }

        public override void SetUpArgPosToCompatability() {
            argPosToCompatability = new List<List<Type>> {
                new List<Type> {
                    typeof(INumberDataType), typeof(MathInstruction)
                },
                 new List<Type> {
                    typeof(INumberDataType), typeof(MathInstruction)
                }
            };
        }

        public override void SetUpArgDescriptionList() {
            argDescriptionList = new List<string> { "Left number", "Right Number" };
        }
    }
}
