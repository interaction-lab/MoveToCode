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
            SetValue(size); //TODO: Probably should reconfigure this
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

        /*public override void SetValue(object valIn) {
            value = (int)valIn;
        }*/

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
            if(index < mySize) {
                internalArray[index] = valIn;
            } else {
                throw new InvalidOperationException("Trying to read beyond array length");
            }
        }

        public object GetValueAtIndex(int index) {
            return internalArray[index].GetValue();
        }

        public override void EvaluateArgumentList() {
            numFilledElements = 0;
            for (int i = 0; i < mySize; i++) {
                if (GetArgumentAt(i)?.EvaluateArgument() == null) {
                    internalArray[i] = null;
                } else {
                    if (((GetArgumentAt(i).EvaluateArgument() as BasicDataType) != internalArray[i]) && internalArray[i] != null) {
                        //do nothing
                    } else {
                        internalArray[i] = GetArgumentAt(i).EvaluateArgument() as BasicDataType;
                        SetArrayType(internalArray[i].GetType());
                    }
                    numFilledElements++;
                }
            }
        }

        public override List<Type> GetArgCompatibilityAtPos(int pos) {
            EvaluateArgumentList();
            if (argPosToCompatability == null || GetNumFilledElements() <= 1) {
                SetUpArgPosToCompatability();
            }
            return argPosToCompatability[pos];
        }

        public override void SetUpArgPosToCompatability() {
            argPosToCompatability = new List<List<Type>> { };
            if(myType == null || (GetNumFilledElements() < 1)) {
                for (int i = 0; i < GetSize(); i++) {
                    argPosToCompatability.Add(new List<Type> {
                    typeof(BasicDataType)
                    });
                }
            } else {
                for (int i = 0; i < GetSize(); i++) {
                    argPosToCompatability.Add(new List<Type> {
                    myType
                    });
                }
            }
        }

        public override void SetUpArgDescriptionList() {
            argDescriptionList = new List<string> { };
            for(int i = 0; i < GetSize(); i++) {
                argDescriptionList.Add("Element" + i.ToString());
            }
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

