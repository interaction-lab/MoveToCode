using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class CodeBlock : MonoBehaviour {
        Instruction myInstruction;
        CodeBlock nextCodeBlock;
        List<CodeBlock> argumentCodeBlocks;

        private void Start() {
            ResizeArgumentCodeBlocks(myInstruction.GetNumArguments());
        }

        // Public Methods
        public Instruction GetCodeBlockInstruction() {
            return myInstruction;
        }

        public void setNextCodeBlock(CodeBlock newCodeBlock) {
            RemoveNextCodeBlock();
            AddNewNextCodeBlock(newCodeBlock);
        }

        public void setArgumentBlockAt(CodeBlock arg, int position) {
            myInstruction.SetArgumentAt(arg.GetCodeBlockInstruction(), position);
        }

        public void RemoveNextCodeBlock() {
            nextCodeBlock?.transform.SetParent(CodeBlockManager.instance.transform);
        }

        // Private Helpers
        private void AddNewNextCodeBlock(CodeBlock newCodeBlock) {
            nextCodeBlock = newCodeBlock;
            if (newCodeBlock) {
                newCodeBlock.transform.SetParent(transform);
                newCodeBlock.transform.localPosition = Vector3.down;
            }
        }

        private void SetNextInstruction(Instruction iIn) {
            myInstruction.SetNextInstruction(iIn);
        }

        private void ResizeArgumentCodeBlocks(int numArgs) {
            while (argumentCodeBlocks.Count < numArgs) {
                argumentCodeBlocks.Add(null);
            }
        }
    }
}