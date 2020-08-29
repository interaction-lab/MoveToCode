using System;
using System.Collections.Generic;

namespace MoveToCode {
    public enum IARG {
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
        protected Dictionary<IARG, HashSet<Type>> argCompatabilityDict;

        public abstract IDataType EvaluateArgument();
        public virtual void ResestInternalState() { }

        public virtual string DescriptiveInstructionToString() { return ""; }
        public virtual void EvaluateArgumentList() { }
        public virtual void SetUpArgCompatabilityDict() { }

        public CodeBlock GetCodeBlock() {
            return myCodeBlock;
        }

        public IArgument(CodeBlock cbIn) {
            myCodeBlock = cbIn;
            ResestInternalState();
        }

        public Dictionary<IARG, HashSet<Type>> GetArgCompatabilityDict() {
            if (argCompatabilityDict == null) {
                SetUpArgCompatabilityDict();
            }
            return argCompatabilityDict;
        }

        public int GetNumArguments() {
            return GetArgCompatabilityDict().Count;
        }

        public IArgument GetArgument(IARG iARGIn) {
            return myCodeBlock.GetArgumentFromDict(iARGIn);
        }

        public virtual HashSet<Type> GetArgCompatibility(IARG argDescription) {
            if (argCompatabilityDict == null) {
                SetUpArgCompatabilityDict();
            }
            if (argCompatabilityDict.ContainsKey(argDescription))
                return argCompatabilityDict[argDescription];
            return null;
        }


    }
}