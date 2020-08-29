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

        public override void EvaluateArgumentsOfInstruction() {
            if (GetArgument(SNAPCOLTYPEDESCRIPTION.Array)?.EvaluateArgument().GetType() != typeof(ArrayDataStructure)) {
                arr = null;
            }
            else {
                arr = GetArgument(SNAPCOLTYPEDESCRIPTION.Array)?.EvaluateArgument() as ArrayDataStructure;
                //arr.EvaluateArgumentsOfInstruction();
            }
            index = GetArgument(SNAPCOLTYPEDESCRIPTION.ArrayElement)?.EvaluateArgument() as IntDataType;
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            indexVal = (int)index.GetValue();
            arrValAtIndex = arr?.GetValueAtIndex(indexVal);
            if (arr?.GetArrayType() == typeof(IntDataType)) {
                return new InstructionReturnValue(new IntDataType(null, (int)arrValAtIndex), null);
            }
            else if (arr.GetArrayType() == typeof(FloatDataType)) {
                return new InstructionReturnValue(new FloatDataType((float)arrValAtIndex), null);
            }
            else if (arr.GetArrayType() == typeof(StringDataType)) {
                return new InstructionReturnValue(new StringDataType(null, (string)arrValAtIndex), null);
            }
            else if (arr.GetArrayType() == typeof(CharDataType)) {
                char c = Convert.ToChar(arrValAtIndex);
                return new InstructionReturnValue(new CharDataType(null, c), null);
            }
            else {
                bool b = Convert.ToBoolean(arrValAtIndex);
                return new InstructionReturnValue(new BoolDataType(null, b), null);
            }
        }

        public void SetArrayValue(IDataType valIn) {
            EvaluateArgumentsOfInstruction();
            indexVal = (int)index.GetValue();
            arr.SetValueAtIndex(indexVal, valIn);
        }

        public override string ToString() {
            return "";
        }


        public override string DescriptiveInstructionToString() {
            return string.Join("", GetArgument(SNAPCOLTYPEDESCRIPTION.Array)?.DescriptiveInstructionToString(), " ", "[", " ", GetArgument(SNAPCOLTYPEDESCRIPTION.ArrayElement)?.DescriptiveInstructionToString(), "]");
        }
    }
}