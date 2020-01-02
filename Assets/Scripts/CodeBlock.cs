using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;


namespace MoveToCode {
    public abstract class CodeBlock : MonoBehaviour {
        protected Instruction myInstruction;
        protected IDataType myData;
        CodeBlock nextCodeBlock;
        List<CodeBlock> argumentCodeBlocks;
        TextMeshPro textMesh;
        ManipulationHandler manipHandler;
        NearInteractionGrabbable nearInteractionGrabbable;
        CodeBlockSnap codeBlockSnap;

        // Abstract Methods
        protected abstract void SetInstructionOrData();
        public abstract bool IsInstructionCodeBlock();
        public abstract bool IsDataCodeBlock();

        // Start Up
        private void Awake() {
            // MRTK components to add
            manipHandler = gameObject.AddComponent(typeof(ManipulationHandler)) as ManipulationHandler;
            nearInteractionGrabbable = gameObject.AddComponent(typeof(NearInteractionGrabbable)) as NearInteractionGrabbable;

            // Other components
            codeBlockSnap = gameObject.AddComponent(typeof(CodeBlockSnap)) as CodeBlockSnap;

            // Setup
            argumentCodeBlocks = new List<CodeBlock>();
            SetInstructionOrData();
            myInstruction?.SetCodeBlock(this);
            myData?.SetCodeBlock(this);
            if (IsInstructionCodeBlock()) {
                argumentCodeBlocks.Resize(myInstruction.GetNumArguments());
            }

            UpdateText();
        }

        // Public Methods
        public Instruction GetInstruction() {
            return myInstruction;
        }

        public IArgument GetArgumentAt(int position) {
            return myInstruction.GetArgumentAt(position);
        }

        public void SetValueOfData(IDataType dTIn) {
            Assert.IsTrue(IsDataCodeBlock());
            myData.SetValue(dTIn.GetValue());
            UpdateText();
        }

        public void SetNextCodeBlock(CodeBlock newInstructionCodeBlock) {
            Assert.IsTrue(IsInstructionCodeBlock());
            RemoveNextCodeBlock();
            newInstructionCodeBlock.RemoveFromParentBlock();
            AddNewNextCodeBlock(newInstructionCodeBlock);
        }

        public void SetArgumentBlockAt(CodeBlock newArgumentCodeBlock, int position) {
            Assert.IsTrue(IsInstructionCodeBlock());
            RemoveArgumentAt(position);
            newArgumentCodeBlock.RemoveFromParentBlock();
            AddNewArgumentAt(newArgumentCodeBlock, position);
            UpdateText();
        }

        public bool IsMyNextInstruction(Instruction iIn) {
            return myInstruction.GetNextInstruction() == iIn;
        }

        public int GetPositionOfArgument(IArgument iArgIn) {
            int index = 0;
            foreach (CodeBlock codeBlock in argumentCodeBlocks) {
                if (codeBlock.GetArgumentValueOfCodeBlock() == iArgIn) {
                    return index;
                }
                ++index;
            }
            Assert.IsTrue(false); // Should be able to find argument, must call IsMyNextInstruction first
            return -1; // Will never get here, put so VS stops complaining
        }



        // Note: This is slightly inefficienct approach but makes it so you don't have to keep track of as much
        public void RemoveFromParentBlock() {
            CodeBlock parentCodeBlock = transform.parent?.GetComponent<CodeBlock>();
            if (parentCodeBlock != null) {
                if (IsInstructionCodeBlock() && parentCodeBlock.IsMyNextInstruction(myInstruction)) {
                    parentCodeBlock.SetNextCodeBlock(null);
                }
                else {
                    parentCodeBlock.RemoveArgumentAt(
                        parentCodeBlock.GetPositionOfArgument(
                            GetArgumentValueOfCodeBlock()));
                }
                parentCodeBlock.UpdateText();
            }
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
                newArgumentCodeBlock.transform.localPosition = Vector3.right *
                    (newArgumentCodeBlock.transform.localScale.x + 0.25f)
                    * (position + 1); // TODO: this placement
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
            SetNextInstruction(null);
            nextCodeBlock?.transform.SetParent(CodeBlockManager.instance.transform);
            nextCodeBlock = null;
        }

        private void RemoveArgumentAt(int position) {
            SetArgumentAt(null, position);
            argumentCodeBlocks[position]?.transform.SetParent(CodeBlockManager.instance.transform);
            argumentCodeBlocks[position] = null;
        }

        private IArgument GetArgumentValueOfCodeBlock() {
            if (IsInstructionCodeBlock()) {
                return myInstruction;
            }
            return myData;
        }

        // This is needed to wait for the gameobject to spawn
        private IEnumerator UpdateTextNextFrame() {
            yield return new WaitForEndOfFrame();
            UpdateText();
        }

        private void UpdateText() {
            if (textMesh == null) {
                textMesh = GetComponentInChildren<TextMeshPro>(true);
            }
            if (textMesh == null) {
                Object codeBlockTextPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/CodeBlockText.prefab", typeof(Object));
                GameObject codeBlockTextGameObject = (GameObject)GameObject.Instantiate(
                    codeBlockTextPrefab, transform);
                StartCoroutine(UpdateTextNextFrame());
            }
            else {
                textMesh.SetText(ToString());
                transform.parent.GetComponent<CodeBlock>()?.UpdateText();
            }
        }
    }
}