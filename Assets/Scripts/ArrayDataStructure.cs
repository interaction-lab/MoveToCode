using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ArrayDataStructure : IDataType {
        int mySize;
        int numFilledElements;
        IDataType[] internalArray;
        Type myType;

        public ArrayDataStructure(CodeBlock cbIn) : base(cbIn) { }
        public ArrayDataStructure(CodeBlock cbIn, int size) : base(cbIn) {
            SetSize(size);
            SetArrayType(null);
            SetNumFilledElements(0);
            internalArray = new IDataType[mySize];
        }

        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is ArrayDataStructure) {
                return GetValue() == (otherVal as ArrayDataStructure).GetValue();
            }
            throw new InvalidOperationException("Trying to compare Array to non Array");
        }

        public override object GetValue() {
            return internalArray;
        }


        //set length of array, should only be called in constructor
        private void SetSize(int sizeIn) {
            mySize = sizeIn;
        }

        public int GetSize() {
            return mySize;
        }

        public void SetNumFilledElements(int elements) {
            numFilledElements = elements;
        }

        public int GetNumFilledElements() {
            return numFilledElements;
        }

        private void SetArrayType(Type inType) {
            myType = inType;
        }

        public Type GetArrayType() {
            return myType;
        }

        public void SetValueAtIndex(int index, IDataType valIn) {
            if (index < GetSize()) {
                internalArray[index] = valIn;
            }
            else {
                throw new InvalidOperationException("Trying to read beyond array length");
            }
        }

        public object GetValueAtIndex(int index) {
            return internalArray[index].GetValue();
        }

        public override HashSet<Type> GetArgCompatibility(IARG argDescription) {
            EvaluateArgumentList();
            if (argCompatabilityDict == null || GetNumFilledElements() <= 1) {
                SetUpArgCompatabilityDict();
            }
            return argCompatabilityDict[argDescription];
        }

        public override void SetUpArgCompatabilityDict() { //TODO: do I have to worry about nullity, BasicDataType vs. myType?
            argCompatabilityDict = new Dictionary<IARG, HashSet<Type>> {
                { IARG.ArrayElement, new HashSet<Type> { typeof(BasicDataType), myType }  }
            };
        }

        public override void ResestInternalState() {
            for (int i = 0; i < GetSize(); i++) {
                internalArray[i] = null;
            }
        }

        public override string ToString() {
            return "";
        }

        public override Type GetCastType() {
            return typeof(Array);
        }
    }
}

