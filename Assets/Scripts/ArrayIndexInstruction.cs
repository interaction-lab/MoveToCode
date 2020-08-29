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
            if (GetArgument(IARG.Array)?.EvaluateArgument().GetType() != typeof(ArrayDataStructure)) {
                arr = null;
            }
            else {
                arr = GetArgument(IARG.Array)?.EvaluateArgument() as ArrayDataStructure;
                arr.EvaluateArgumentList();
            }
            index = GetArgument(IARG.ArrayElement)?.EvaluateArgument() as IntDataType;
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentList();
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
            EvaluateArgumentList();
            indexVal = (int)index.GetValue();
            arr.SetValueAtIndex(indexVal, valIn);
        }

        public override string ToString() {
            return "";
        }


        public override void SetUpArgCompatabilityDict() {
            argCompatabilityDict = new Dictionary<IARG, HashSet<Type>> {
                { IARG.Array, new HashSet<Type> { typeof(Variable) }  },
                { IARG.ArrayElement, new HashSet<Type> {  typeof(IntDataType), typeof(MathInstruction) }  }
            };
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", GetArgument(IARG.Array)?.DescriptiveInstructionToString(), " ", "[", " ", GetArgument(IARG.ArrayElement)?.DescriptiveInstructionToString(), "]");
        }
    }
}