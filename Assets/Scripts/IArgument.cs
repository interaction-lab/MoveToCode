using System;
using System.Collections.Generic;

namespace MoveToCode {
    //public enum string {
    //    Next,
    //    Array,
    //    Value,
    //    Nested,
    //    Variable,
    //    NotFound,
    //    Printable,
    //    LeftNumber,
    //    RightNumber,
    //    Conditional,
    //    ArrayElement,
    //    ArrayDataStructure,
    //    LeftOfConditional,
    //    RightOfConditional,
    //}
    
    public abstract class IArgument {
        CodeBlock myCodeBlock;

        protected Dictionary<string, SnapCollider> argToSnapColliderDict = new Dictionary<string, SnapCollider>();

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

        internal void RegisterSnapCollider(string snapColDescIn, SnapCollider snapCollider) {
            GetArgToSnapColliderDict()[snapColDescIn] = snapCollider;
        }

        public Dictionary<string, SnapCollider> GetArgToSnapColliderDict() {
            return argToSnapColliderDict;
        }

        public int GetNumArguments() {
            return GetArgToSnapColliderDict().Count;
        }

        public IArgument GetArgument(string iARGIn) {
            return GetArgToSnapColliderDict()[iARGIn]?.GetMyCodeBlockArg()?.GetMyIArgument();
        }

        public static Dictionary<string, HashSet<Type>> iArgCompatabilityDict =
            new Dictionary<string, HashSet<Type>> {
                { "Next", new HashSet<Type> { typeof(StandAloneInstruction) }  },
                { "LeftOfConditional", new HashSet<Type> {  typeof(BasicDataType), typeof(MathInstruction), typeof(ArrayIndexInstruction) }  },
                { "RightOfConditional", new HashSet<Type> {  typeof(BasicDataType), typeof(MathInstruction), typeof(ArrayIndexInstruction) }  },
                { "ArrayElement", new HashSet<Type> { typeof(BasicDataType) } },
                { "Array", new HashSet<Type> { typeof(Variable) }  },
                { "ArrayDataStructure", new HashSet<Type> {  typeof(ArrayDataStructure) }  },
                { "Variable", new HashSet<Type> { typeof(Variable) , typeof(ArrayIndexInstruction) }  },
                { "Nested", new HashSet<Type> { typeof(StandAloneInstruction) }  },
                { "Conditional", new HashSet<Type> {  typeof(ConditionalInstruction) }  },
                { "Value", new HashSet<Type> { typeof(IDataType), typeof(MathInstruction), typeof(ConditionalInstruction), typeof(ArrayIndexInstruction) }  },
                { "Printable", new HashSet<Type> { typeof(IDataType), typeof(MathInstruction), typeof(ConditionalInstruction), typeof(ArrayIndexInstruction) }  },
                { "LeftNumber", new HashSet<Type> {  typeof(INumberDataType), typeof(MathInstruction) }  },
                { "RightNumber", new HashSet<Type> {  typeof(INumberDataType), typeof(MathInstruction) }  }
            };


        public virtual HashSet<Type> GetArgCompatibility(string argDescription) {
            return iArgCompatabilityDict[argDescription];
        }


    }
}