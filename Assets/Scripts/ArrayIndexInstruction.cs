using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class ArrayIndexInstruction : Instruction {
        protected ArrayDataStructure arr;
        protected IntDataType index;

        public ArrayIndexInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
            if(GetArgumentAt(0)?.EvaluateArgument().GetType() != typeof(ArrayDataStructure)) {
                arr = null;
            } else {
                arr = GetArgumentAt(0)?.EvaluateArgument() as ArrayDataStructure;
            }
            index = GetArgumentAt(1)?.EvaluateArgument() as IntDataType;
        }

        /*public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            return new InstructionReturnValue(new IDataType(null, index.GetValue())), null);
        }*/

        public override int GetNumArguments() {
            return 2;
        }

        public override string ToString() {
            return "";
        }

        public override void SetUpArgPosToCompatability() {
            argPosToCompatability = new List<List<Type>> {
                new List<Type> {
                    typeof(Variable)
                },
                new List<Type> {
                    typeof(IntDataType), typeof(MathInstruction)
                }
            };
        }

        public override void SetUpArgDescriptionList() {
            argDescriptionList = new List<string> { "Array", "Index" };
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", GetArgumentAt(0)?.DescriptiveInstructionToString(), " ", "[", " ", GetArgumentAt(1)?.DescriptiveInstructionToString(), "]");
        }
    }
}