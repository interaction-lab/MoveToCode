using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ArrayDataStructure : IDataType {
        int mySize;
        IDataType[] internalArray;

        public ArrayDataStructure(CodeBlock cbIn) : base(cbIn) { }
        public ArrayDataStructure(CodeBlock cbIn, int size) : base(cbIn) {
            SetSize(size);
            internalArray = new IDataType[mySize];
        }

        //TODO: Fix this
        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is ArrayDataStructure) {
                return (string)GetValue() == (string)(otherVal as ArrayDataStructure).GetValue();
            }
            throw new InvalidOperationException("Trying to compare Array to non Array");
        }

        public void SetValue(object valIn) {
            throw new InvalidOperationException("Trying to set a value in the array without specifying an index");
        }

        public override object GetValue() {
            return internalArray;
        }

        public override int GetNumArguments() {
            return GetSize();
        }

        //set length of array, should only be called in constructor
        private void SetSize(int sizeIn) {
            mySize = sizeIn;
        }

        //get size of array (not the number of elements of the array)
        public int GetSize() {
            return mySize;
        }

        public void SetValueAtIndex(int index) {
            if(index < mySize) {
                internalArray[index] = GetArgumentAt(index).EvaluateArgument();
            } else {
                throw new InvalidOperationException("Trying to read beyond array length");
            }
        }

        public IDataType GetValueAtIndex(int index) {
            return internalArray[index];
        }

        public override void EvaluateArgumentList() {
            for(int i = 0; i < mySize; i++) {
                internalArray[i] = GetArgumentAt(i)?.EvaluateArgument() as BasicDataType;
            }
        }

        public override void SetUpArgPosToCompatability() {
            argPosToCompatability = new List<List<Type>> { };
            for (int i = 0; i < GetSize(); i++) {
                argPosToCompatability.Add(new List<Type> {
                    typeof(IDataType)
                });
            }
        }

        //TODO: Fix this
        public override void SetUpArgDescriptionList() {
            argDescriptionList = new List<string> { "Left side of condtional", "Right Side of Conditional" };
        }

        public override string ToString() {
            return "";
        }

        public override Type GetCastType() {
            return typeof(Array);
        }
    }
}

