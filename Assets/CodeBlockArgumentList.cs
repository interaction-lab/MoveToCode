using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class CodeBlockArgumentList : MonoBehaviour {
        List<CodeBlock> argList;
        CodeBlock myCodeBlock;

        public void SetUp(CodeBlock cbIn) {
            myCodeBlock = cbIn;
        }

        public List<CodeBlock> GetArgListCodeBlocks() {
            if (argList == null) {
                argList = new List<CodeBlock>(GetNumArguments());
            }
            return argList;
        }

        public IArgument GetArgAsIArgumentAt(int pos) {
            return GetArgListCodeBlocks()[pos].GetMyInternalIArgument();
        }

        public IEnumerable GetArgListAsIArguments() {
            for (int i = 0; i < GetNumArguments(); ++i) {
                yield return GetArgAsIArgumentAt(i);
            }
        }

        public void SetArgCodeBlockAt(CodeBlock cbIn, int argPosition, Vector3 newLocalPosition) {
            // set and remove here
            RemoveArgumentAt(argPosition);
            newArgumentCodeBlock?.RemoveFromParentBlock();
            AddNewArgumentAt(newArgumentCodeBlock, argPosition, newLocalPosition);
            GetArgListCodeBlocks()[pos] = cbIn;
        }

        public int GetNumArguments() {
            return myCodeBlock.GetMyInternalIArgument().GetNumArguments();
        }



    }
}
