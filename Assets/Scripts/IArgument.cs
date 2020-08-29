using System;
using System.Collections.Generic;

namespace MoveToCode {
    public enum SNAPCOLTYPEDESCRIPTION {
        Next,
        Array,
        Value,
        Nested,
        Variable,
        NotFound,
        Printable,
        LeftNumber,
        RightNumber,
        Conditional,
        ArrayElement,
        ArrayDataStructure,
        LeftOfConditional,
        RightOfConditional,
    }

    public abstract class IArgument {
        CodeBlock myCodeBlock;
        // TODO: this should just be a constant
        protected Dictionary<SNAPCOLTYPEDESCRIPTION, SnapCollider> argToSnapColliderDict = new Dictionary<SNAPCOLTYPEDESCRIPTION, SnapCollider>();

        public abstract IDataType EvaluateArgument();
        public abstract void ResestInternalState();
        public abstract void EvaluateArgumentList();
        public abstract void SetUpArgToSnapColliderDict();
        public abstract string DescriptiveInstructionToString();


        public CodeBlock GetCodeBlock() {
            return myCodeBlock;
        }

        public IArgument(CodeBlock cbIn) {
            myCodeBlock = cbIn;
            ResestInternalState();
        }

        internal void RegisterSnapCollider(SNAPCOLTYPEDESCRIPTION snapColDescIn, SnapCollider snapCollider) {
            GetArgToSnapColliderDict()[snapColDescIn] = snapCollider;
        }

        public Dictionary<SNAPCOLTYPEDESCRIPTION, SnapCollider> GetArgToSnapColliderDict() {
            return argToSnapColliderDict;
        }

        public int GetNumArguments() {
            return GetArgToSnapColliderDict().Count;
        }

        public IArgument GetArgument(SNAPCOLTYPEDESCRIPTION iARGIn) {
            return myCodeBlock.GetArgumentFromDict(iARGIn);
        }


        public static Dictionary<SNAPCOLTYPEDESCRIPTION, HashSet<Type>> iArgCompatabilityDict =
            new Dictionary<SNAPCOLTYPEDESCRIPTION, HashSet<Type>> {
                { SNAPCOLTYPEDESCRIPTION.LeftOfConditional, new HashSet<Type> {  typeof(BasicDataType), typeof(MathInstruction), typeof(ArrayIndexInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.RightOfConditional, new HashSet<Type> {  typeof(BasicDataType), typeof(MathInstruction), typeof(ArrayIndexInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.ArrayElement, new HashSet<Type> { typeof(BasicDataType) } },
                 { SNAPCOLTYPEDESCRIPTION.Array, new HashSet<Type> { typeof(Variable) }  },
                { SNAPCOLTYPEDESCRIPTION.ArrayElement, new HashSet<Type> {  typeof(IntDataType), typeof(MathInstruction) }  }
            };


        public virtual HashSet<Type> GetArgCompatibility(SNAPCOLTYPEDESCRIPTION argDescription) {
            return iArgCompatabilityDict[argDescription];
        }


    }
}