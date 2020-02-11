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
                argList = new List<CodeBlock>(new CodeBlock[GetNumArguments()]);
            }
            return argList;
        }

        public IArgument GetArgAsIArgumentAt(int pos) {
            return GetArgListCodeBlocks()[pos].GetMyInternalIArgument();
        }

        public List<IArgument> GetArgListAsIArguments() {
            List<IArgument> result = new List<IArgument>(new IArgument[GetNumArguments()]);
            for (int i = 0; i < GetNumArguments(); ++i) {
                result[i] = GetArgAsIArgumentAt(i);
            }
            return result;
        }

        public void SetArgCodeBlockAt(CodeBlock newArgumentCodeBlock, int pos, Vector3 newLocalPosition) {
            // set and remove here
            RemoveArgumentAt(pos); // this should be here
            newArgumentCodeBlock?.RemoveFromParentBlock(); // this should be in codeblock
            AddNewArgumentAt(newArgumentCodeBlock, pos, newLocalPosition);
            GetArgListCodeBlocks()[pos] = newArgumentCodeBlock;
        }

        public int GetNumArguments() {
            return myCodeBlock.GetMyInternalIArgument().GetNumArguments();
        }

        // Private methods, reconsider if you need to make these public
        private void AddNewArgumentAt(CodeBlock newArgumentCodeBlock, int pos, Vector3 newLocalPosition) {
            if (newArgumentCodeBlock) {
                newArgumentCodeBlock.transform.SnapToParent(transform, newLocalPosition);
            }
            GetArgListCodeBlocks()[pos] = newArgumentCodeBlock;
        }

        private void RemoveArgumentAt(int position) {
            if (GetArgListCodeBlocks()[position] != null) {
                argList[position].transform.localPosition = argList[position].transform.localPosition + new Vector3(0.25f, 0.25f, 1.25f); // TODO: This Placement
                argList[position].transform.SetParent(CodeBlockManager.instance.transform);
                argList[position] = null;
            }
        }

    }
}
