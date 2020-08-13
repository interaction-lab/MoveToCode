using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public enum IArg {        
        Next,
        Nested,
        NotFound,
        Printable,
        Conditional,
        ArrayElement,
        LeftOfConditional,
        RightOfConditional
    }

    public abstract class IArgument {
        CodeBlock myCodeBlock;
        protected Dictionary<IArg, HashSet<Type>> argCompatabilityDict;
        //protected List<List<Type>> argPosToCompatability; 
        //protected List<string> argDescriptionList; 

        public abstract IDataType EvaluateArgument();
        public virtual void ResestInternalState() { }
        public abstract int GetNumArguments();
        public virtual string DescriptiveInstructionToString() { return ""; }
        public virtual void EvaluateArgumentList() { }
        //public virtual void SetUpArgPosToCompatability() { }
        //public virtual void SetUpArgDescriptionList() { }
        public virtual void SetUpArgCompatabilityDict() { }

        public CodeBlock GetCodeBlock() {
            return myCodeBlock;
        }

        public IArgument(CodeBlock cbIn) {
            myCodeBlock = cbIn;
            ResestInternalState();
        }


        //public virtual List<Type> GetArgCompatibilityAtPos(int pos) {
        //    if (argPosToCompatability == null) {
        //        SetUpArgPosToCompatability();
        //    }
        //    return argPosToCompatability[pos];
        //}
        public virtual HashSet<Type> GetArgCompatibility(IArg argDescription) {
            return argCompatabilityDict[argDescription];
        }



        //public List<IArgument> GetArgumentListAsIArgs() {
        //    return GetCodeBlock().GetArgumentListAsIArgs();
        //}
        public Dictionary<IArg, IArgument> GetArgDictAsIArgs() {
            return GetCodeBlock().GetArgDictAsIArgs();
        }

        //public IArgument GetArgumentAt(int pos) {
        //    return GetArgumentListAsIArgs()[pos];
        //}
        public IArgument GetArgument(IArg argDescription) {
            return GetArgDictAsIArgs()[argDescription];
        }


        //public List<string> GetArgListDescription() {
        //    if (argDescriptionList == null) {
        //        SetUpArgDescriptionList();
        //    }
        //    return argDescriptionList;
        //}

        //public void PrintArgDescriptions() {
        //    int i = 0;
        //    foreach (string s in GetArgListDescription()) {
        //        Debug.Log(string.Join("", "Arg[", i.ToString(), "] = ", s));
        //        ++i;
        //    }
        //}
    }
}