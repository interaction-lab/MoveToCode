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

        public CodeBlock GetArgAsCodeBlockAt(int pos) {
            return GetArgListCodeBlocks()[pos];
        }

        public IArgument GetArgAsIArgumentAt(int pos) {
            return GetArgAsCodeBlockAt(pos)?.GetMyInternalIArgument();
        }

        public List<IArgument> GetArgListAsIArguments() {
            List<IArgument> result = new List<IArgument>(new IArgument[GetNumArguments()]);
            for (int i = 0; i < GetNumArguments(); ++i) {
                result[i] = GetArgAsIArgumentAt(i);
            }
            return result;
        }

        public void SetArgCodeBlockAt(CodeBlock newArgumentCodeBlock, int pos, bool humanDidIt) {
            RemoveArgumentAt(pos, humanDidIt);
            newArgumentCodeBlock?.RemoveFromParentBlock(humanDidIt);
            AddNewArgumentAt(newArgumentCodeBlock, pos, humanDidIt);
        }

        public int GetNumArguments() {
            return myCodeBlock.GetMyInternalIArgument().GetNumArguments();
        }

        // Private methods, reconsider if you need to make these public
        private void AddNewArgumentAt(CodeBlock newArgumentCodeBlock, int position, bool humanDidIt) {
            GetArgListCodeBlocks()[position] = newArgumentCodeBlock;
            if (newArgumentCodeBlock != null) {
                if (humanDidIt) {
                    LoggingManager.instance.UpdateLogColumn(SnapLoggingManager.GetSnapToColName(),
                                                             string.Join("",
                                                             "Add ",
                                                             argList[position].name,
                                                             " to ",
                                                             myCodeBlock.name,
                                                             " at ",
                                                             position.ToString()));
                }
                AudioManager.instance.PlaySoundAtObject(gameObject, AudioManager.snapAudioClip);
                myCodeBlock.GetCodeBlockObjectMesh().ResizeChain();
            }
        }

        private void RemoveArgumentAt(int position, bool humanDidIt) {
            if (GetArgListCodeBlocks()[position] != null) {
                if (humanDidIt) {
                    LoggingManager.instance.UpdateLogColumn(SnapLoggingManager.GetSnapRemoveFromColName(),
                                                             string.Join("",
                                                             "Remove ",
                                                             argList[position].name,
                                                             " from ",
                                                             myCodeBlock.name,
                                                             " at ",
                                                             position.ToString()));
                }
                AudioManager.instance.PlaySoundAtObject(gameObject, AudioManager.popAudioClip);
                if (CodeBlockSnap.lastDraggedCBS != myCodeBlock.GetCodeBlockSnap()) {
                    argList[position].transform.localPosition = argList[position].transform.localPosition + new Vector3(0.25f, 1.1f, 1.25f);
                }
                argList[position].transform.SnapToCodeBlockManager();
                argList[position] = null;
                myCodeBlock.GetCodeBlockObjectMesh().ResizeChain();
            }
        }

    }
}
