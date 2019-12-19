using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public abstract class CodeBlock : MonoBehaviour {
        protected Instruction myInstruction;
        protected IDataType myData;
        CodeBlock nextCodeBlock;
        List<CodeBlock> argumentCodeBlocks;

        // Abstract Methods
        protected abstract void SetInstructionOrData();

        // Start Up
        private void Awake() {
            argumentCodeBlocks = new List<CodeBlock>();
            SetInstructionOrData();
            myInstruction?.SetCodeBlock(this);
            myData?.SetCodeBlock(this);
            argumentCodeBlocks.Resize(myInstruction.GetNumArguments());
        }

        // Public Methods
        public Instruction GetInstruction() {
            return myInstruction;
        }

        public IArgument GetArgumentAt(int position) {
            return myInstruction.GetArgumentAt(position);
        }

        public void SetNextCodeBlock(CodeBlock newInstructionCodeBlock) {
            RemoveNextCodeBlock();
            AddNewNextCodeBlock(newInstructionCodeBlock);
        }

        public void SetArgumentBlockAt(CodeBlock newArgumentCodeBlock, int position) {
            RemoveArgumentAt(position);
            AddNewArgumentAt(newArgumentCodeBlock, position);
        }

        public bool IsInstructionCodeBlock() {
            return myInstruction != null;
        }

        public bool IsDataCodeBlock() {
            return myData != null;
        }

        // Private Helpers
        // If you find yourself making these public, 
        // then you should reconsider what you are doing
        private void AddNewNextCodeBlock(CodeBlock newCodeBlock) {
            nextCodeBlock = newCodeBlock;
            if (newCodeBlock) {
                newCodeBlock.transform.SetParent(transform);
                newCodeBlock.transform.localPosition = Vector3.down; // TODO: once arg placing is done, update this for better placement
            }
            SetNextInstruction(newCodeBlock.GetInstruction());
        }

        private void AddNewArgumentAt(CodeBlock newArgumentCodeBlock, int position) {
            // need to update instruction arguments
            argumentCodeBlocks[position] = newArgumentCodeBlock;
            if (newArgumentCodeBlock) {
                newArgumentCodeBlock.transform.SetParent(transform);
                newArgumentCodeBlock.transform.localPosition = Vector3.right * (position + 1); // TODO: this placement
            }
            myInstruction.SetArgumentAt(newArgumentCodeBlock.GetArgumentValueOfCodeBlock(), position);
        }

        private void SetNextInstruction(Instruction iIn) {
            myInstruction.SetNextInstruction(iIn);
        }

        private void SetArgumentAt(IArgument newArg, int position) {
            myInstruction.SetArgumentAt(newArg, position);
        }

        private void RemoveNextCodeBlock() {
            nextCodeBlock?.transform.SetParent(CodeBlockManager.instance.transform);
            nextCodeBlock = null;
        }

        private void RemoveArgumentAt(int position) {
            argumentCodeBlocks[position]?.transform.SetParent(CodeBlockManager.instance.transform);
            argumentCodeBlocks[position] = null;
        }

        private IArgument GetArgumentValueOfCodeBlock() {
            if (IsInstructionCodeBlock()) {
                return myInstruction;
            }
            return myData;
        }
    }
}