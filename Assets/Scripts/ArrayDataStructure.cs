using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ArrayDataStructure : IDataType {
        int size;
        IDataType[] internalArray;
        Type arrayType;
        List<List<Type>> argPosToCompatability;
        List<string> argDescriptionList;

        public ArrayDataStructure(CodeBlock cbIn) : base(cbIn) { }
        public ArrayDataStructure(CodeBlock cbIn, int size) : base(cbIn) {
            //SetValue(valIn);
            SetSize(size);
            arrayType = typeof(int); //default
        }

        //TODO: Fix this
        public override bool IsSameDataTypeAndEqualTo(IDataType otherVal) {
            if (otherVal is ArrayDataStructure) {
                return (string)GetValue() == (string)otherVal.GetValue();
            }
            throw new InvalidOperationException("Trying to compare Array to non Array");
        }

        public override void SetValue(object valIn) {
            throw new InvalidOperationException("Trying to set a value in the array without specifying an index");
        }

        public override object GetValue() {
            return 0;
            //throw new InvalidOperationException("Trying to get a value from the array without specifying an index");
        }

        public override int GetNumArguments() {
            return internalArray.Length;
        }

        //set length of array, should only be called in constructor
        public void SetSize(int sizeIn) {
            size = sizeIn;
            internalArray = new IDataType[size];
        }

        //get size of array (not the number of elements of the array)
        public int GetSize() {
            return size;
        }

        public bool Empty() {
            for(int i = 0; i < size; i++) {
                if(internalArray[i] != null) {
                    return false;
                }
            }
            return true;
        }

        public void SetValueAtIndex(int index, IDataType valIn) {
            //first input sets the type of array
            if(Empty()) {
                arrayType = valIn.GetType();
                SetUpArgPosToCompatability();
            }
            if(index < size) {
                internalArray[index] = valIn;
            } else {
                throw new InvalidOperationException("Trying to read beyond array length");
            }
        }

        public void EvaluateArgumentList() {
            for(int i = 0; i < size; i++) {
                internalArray[i] = GetArgumentAt(i)?.EvaluateArgument().GetValue() as IDataType;
            }
            /*if (GetArgumentAt(0) != null && GetArgumentAt(1) != null) {
                leftNum = (float)Convert.ChangeType(GetArgumentAt(0).EvaluateArgument().GetValue(), typeof(float));
                rightNum = (float)Convert.ChangeType(GetArgumentAt(1).EvaluateArgument().GetValue(), typeof(float));
            }*/
        }

        //TODO: maybe operator overload instead?
        public IDataType GetValueAtIndex(int index) {
            return internalArray[index];
        }

        public List<Type> GetArgCompatibilityAtPos(int pos) {
            if (argPosToCompatability == null) {
                SetUpArgPosToCompatability();
            }
            return argPosToCompatability[pos];
        }

        public IArgument GetArgumentAt(int pos) {
            return GetArgumentListAsIArgs()[pos];
        }

        public List<IArgument> GetArgumentListAsIArgs() {
            return GetCodeBlock().GetArgumentListAsIArgs();
        }

        public List<string> GetArgListDescription() {
            if (argDescriptionList == null) {
                SetUpArgDescriptionList();
            }
            return argDescriptionList;
        }

        public void SetUpArgPosToCompatability() {
            argPosToCompatability = new List<List<Type>> { };
            for (int i = 0; i < GetSize(); i++) {
                argPosToCompatability.Add(new List<Type> {
                    arrayType
                });
            }
            /*argPosToCompatability = new List<List<Type>> {
                new List<Type> {
                    typeof(IDataType), typeof(MathInstruction)
                },
                new List<Type> {
                    typeof(IDataType), typeof(MathInstruction)
                }
            };*/
        }

        //TODO: Fix this
        public void SetUpArgDescriptionList() {
            argDescriptionList = new List<string> { "Left side of condtional", "Right Side of Conditional" };
        }


        public override Type GetCastType() {
            return typeof(int);
        }
    }
}

