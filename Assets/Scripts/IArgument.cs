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

        protected Dictionary<SNAPCOLTYPEDESCRIPTION, SnapCollider> argToSnapColliderDict = new Dictionary<SNAPCOLTYPEDESCRIPTION, SnapCollider>();

        public abstract IDataType EvaluateArgument();
        public abstract string DescriptiveInstructionToString();

        public virtual void ResestInternalState() {
        }

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
            return GetArgToSnapColliderDict()[iARGIn]?.GetMyCodeBlockArg()?.GetMyIArgument();
        }

        public static Dictionary<SNAPCOLTYPEDESCRIPTION, HashSet<Type>> iArgCompatabilityDict =
            new Dictionary<SNAPCOLTYPEDESCRIPTION, HashSet<Type>> {
                { SNAPCOLTYPEDESCRIPTION.Next, new HashSet<Type> { typeof(StandAloneInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.LeftOfConditional, new HashSet<Type> {  typeof(BasicDataType), typeof(MathInstruction), typeof(ArrayIndexInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.RightOfConditional, new HashSet<Type> {  typeof(BasicDataType), typeof(MathInstruction), typeof(ArrayIndexInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.ArrayElement, new HashSet<Type> { typeof(BasicDataType) } },
                { SNAPCOLTYPEDESCRIPTION.Array, new HashSet<Type> { typeof(Variable) }  },
                { SNAPCOLTYPEDESCRIPTION.ArrayDataStructure, new HashSet<Type> {  typeof(ArrayDataStructure) }  },
                { SNAPCOLTYPEDESCRIPTION.Variable, new HashSet<Type> { typeof(Variable) , typeof(ArrayIndexInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.Nested, new HashSet<Type> { typeof(StandAloneInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.Conditional, new HashSet<Type> {  typeof(ConditionalInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.Value, new HashSet<Type> { typeof(IDataType), typeof(MathInstruction), typeof(ConditionalInstruction), typeof(ArrayIndexInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.Printable, new HashSet<Type> { typeof(IDataType), typeof(MathInstruction), typeof(ConditionalInstruction), typeof(ArrayIndexInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.LeftNumber, new HashSet<Type> {  typeof(INumberDataType), typeof(MathInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.RightNumber, new HashSet<Type> {  typeof(INumberDataType), typeof(MathInstruction) }  }
            };


        public virtual HashSet<Type> GetArgCompatibility(SNAPCOLTYPEDESCRIPTION argDescription) {
            return iArgCompatabilityDict[argDescription];
        }


    }
}