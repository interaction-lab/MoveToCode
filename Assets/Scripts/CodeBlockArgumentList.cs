using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class CodeBlockArgumentList : MonoBehaviour {
        //List<CodeBlock> argList;
        Dictionary<IARG, CodeBlock> argDict;
        CodeBlock myCodeBlock;

        public void SetUp(CodeBlock cbIn) {
            myCodeBlock = cbIn;
        }

        //public List<CodeBlock> GetArgListCodeBlocks() {
        //    if (argList == null) {
        //        argList = new List<CodeBlock>(new CodeBlock[GetNumArguments()]);
        //    }
        //    return argList;
        //}
        public Dictionary<IARG, CodeBlock> GetArgDictAsCodeBlocks() {
            if (argDict == null)
                argDict = new Dictionary<IARG, CodeBlock>();
            return argDict;
        }


        //public CodeBlock GetArgAsCodeBlockAt(int pos) {
        //    return GetArgListCodeBlocks()[pos];
        //}
        public CodeBlock GetArgAsCodeBlock(IARG argDescription) {
            if (argDict.ContainsKey(argDescription))
                return argDict[argDescription];
            return null;
        }

        //public IArgument GetArgAsIArgumentAt(int pos) {
        //    return GetArgAsCodeBlockAt(pos)?.GetMyIArgument();
        //}
        public IArgument GetArgAsIArg(IARG argDescription) {
            return GetArgAsCodeBlock(argDescription)?.GetMyIArgument();
        }

        //public List<IArgument> GetArgListAsIArguments() {
        //    List<IArgument> result = new List<IArgument>(new IArgument[GetNumArguments()]);
        //    for (int i = 0; i < GetNumArguments(); ++i) {
        //        result[i] = GetArgAsIArgumentAt(i);
        //    }
        //    return result;
        //}
        public Dictionary<IARG, IArgument> GetArgDictAsIArgs() {
            Dictionary<IARG, IArgument> result = new Dictionary<IARG, IArgument>();
            if (argDict == null) {
                argDict = new Dictionary<IARG, CodeBlock>();
                return result;
            }
            foreach (KeyValuePair<IARG, CodeBlock> pair in argDict){
                result[pair.Key] = pair.Value?.GetMyIArgument();
            }
            return result;
        }

        //public void SetArgCodeBlockAt(CodeBlock newArgumentCodeBlock, int pos, bool humanDidIt) {
        //    RemoveArgumentAt(pos, humanDidIt);
        //    newArgumentCodeBlock?.RemoveFromParentBlock(humanDidIt);
        //    AddNewArgumentAt(newArgumentCodeBlock, pos, humanDidIt);
        //}
        public void SetArg(IARG argDescription, CodeBlock newArg, bool humanDidIt) {
            RemoveArg(argDescription, humanDidIt);
            newArg?.RemoveFromParentBlock(humanDidIt);
            AddArg(argDescription, newArg, humanDidIt);
        }

        //literally only for ArrayCodeBlock I know its ugly shut up
        public void SetArrayArg(int pos, CodeBlock block, bool humanDidIt) {
            
        }


        public int GetNumArguments() { 
            return myCodeBlock.GetMyIArgument().GetNumArguments();
        }

        // Private methods, reconsider if you need to make these public
        //private void AddNewArgumentAt(CodeBlock newArgumentCodeBlock, int position, bool humanDidIt) {
        //    GetArgListCodeBlocks()[position] = newArgumentCodeBlock;
        //    if (newArgumentCodeBlock != null) {
        //        if (humanDidIt) {
        //            LoggingManager.instance.UpdateLogColumn(SnapLoggingManager.GetSnapToColName(),
        //                                                     string.Join("",
        //                                                     "Add ",
        //                                                     argList[position].name,
        //                                                     " to ",
        //                                                     myCodeBlock.name,
        //                                                     " at ",
        //                                                     position.ToString()));
        //        }
        //        AudioManager.instance.PlaySoundAtObject(gameObject, AudioManager.snapAudioClip);
        //        myCodeBlock.GetCodeBlockObjectMesh().ResizeChain();
        //    }
        //}
        private void AddArg(IARG argDescription, CodeBlock newArg, bool humanDidIt) {
            GetArgDictAsCodeBlocks()[argDescription] = newArg;
            if (newArg != null) {
                if (humanDidIt) {
                    LoggingManager.instance.UpdateLogColumn(SnapLoggingManager.GetSnapToColName(),
                                                             string.Join("", "Add ", argDescription.ToString(),
                                                                              " to ", myCodeBlock.name));
                }
                AudioManager.instance.PlaySoundAtObject(gameObject, AudioManager.snapAudioClip);
                myCodeBlock.GetCodeBlockObjectMesh().ResizeChain();
            }
        }


        //private void RemoveArgumentAt(int position, bool humanDidIt) {
        //    if (GetArgListCodeBlocks()[position] != null) {
        //        if (humanDidIt) {
        //            LoggingManager.instance.UpdateLogColumn(SnapLoggingManager.GetSnapRemoveFromColName(),
        //                                                     string.Join("",
        //                                                     "Remove ",
        //                                                     argList[position].name,
        //                                                     " from ",
        //                                                     myCodeBlock.name,
        //                                                     " at ",
        //                                                     position.ToString()));
        //        }
        //        AudioManager.instance.PlaySoundAtObject(gameObject, AudioManager.popAudioClip);
        //        if (CodeBlockSnap.lastDraggedCBS != myCodeBlock.GetCodeBlockSnap()) {
        //            argList[position].transform.localPosition = argList[position].transform.localPosition + new Vector3(0.25f, 1.1f, 1.25f);
        //        }
        //        argList[position].transform.SnapToCodeBlockManager();
        //        argList[position] = null;
        //        myCodeBlock.GetCodeBlockObjectMesh().ResizeChain();
        //    }
        //}
        private void RemoveArg(IARG argDescription, bool humanDidIt) {
            if (GetArgDictAsCodeBlocks().ContainsKey(argDescription)) {
                if (humanDidIt) {
                    LoggingManager.instance.UpdateLogColumn(SnapLoggingManager.GetSnapRemoveFromColName(),
                                                             string.Join("",  "Remove ", argDescription.ToString(),
                                                                             " from ",  myCodeBlock.name));
                }
                AudioManager.instance.PlaySoundAtObject(gameObject, AudioManager.popAudioClip);
                if (argDict[argDescription] != null) {
                    if (CodeBlockSnap.lastDraggedCBS != myCodeBlock.GetCodeBlockSnap()) {

                        argDict[argDescription].transform.localPosition = argDict[argDescription].transform.localPosition + new Vector3(0.25f, 1.1f, 1.25f); //check null

                    }
                    argDict[argDescription].transform.SnapToCodeBlockManager();
                    argDict[argDescription] = null;
                    myCodeBlock.GetCodeBlockObjectMesh().ResizeChain();
                }
               

            }
        }

        }
}
