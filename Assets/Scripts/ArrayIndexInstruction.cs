using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ArrayIndexInstruction : Instruction {
        protected ArrayDataStructure arr;
        protected IntDataType index;
        int indexVal;
        object arrValAtIndex;

        public ArrayIndexInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentList() {
            if(GetArgumentAt(0)?.EvaluateArgument().GetType() != typeof(ArrayDataStructure)) {
                arr = null;
            } else {
                arr = GetArgumentAt(0)?.EvaluateArgument() as ArrayDataStructure;
                arr.EvaluateArgumentList();
            }
            index = GetArgumentAt(1)?.EvaluateArgument() as IntDataType;
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
            indexVal = (int)index.GetValue();
            arrValAtIndex = arr.GetValueAtIndex(indexVal);
            if (arr.GetArrayType() == typeof(IntDataType)) {
                return new InstructionReturnValue(new IntDataType(null, (int)arrValAtIndex), null);
            } else if (arr.GetArrayType() == typeof(FloatDataType)) {
                return new InstructionReturnValue(new FloatDataType((float)arrValAtIndex), null);
            } else if (arr.GetArrayType() == typeof(StringDataType)) {
                return new InstructionReturnValue(new StringDataType(null, (string)arrValAtIndex), null);
            } else if (arr.GetArrayType() == typeof(CharDataType)) {
                char c = Convert.ToChar(arrValAtIndex);
                return new InstructionReturnValue(new CharDataType(null, c), null);
            } else {
                return new InstructionReturnValue(new BoolDataType(null, (bool)arrValAtIndex), null);
            }
        }

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