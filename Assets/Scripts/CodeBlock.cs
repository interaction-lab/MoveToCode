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
        Rigidbody rigidBody;
        SnapColliders snapColliders;


        // Abstract Methods
        protected abstract void SetInstructionOrData();
        public abstract bool IsInstructionCodeBlock();
        public abstract bool IsDataCodeBlock();

        // Start Up
        private void Awake() {
            // Set up collision
            rigidBody = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rigidBody.isKinematic = true;
            rigidBody.useGravity = false;
            GetComponent<Collider>().isTrigger = true;

            // MRTK components to add
            manipHandler = gameObject.AddComponent(typeof(ManipulationHandler)) as ManipulationHandler;
            nearInteractionGrabbable = gameObject.AddComponent(typeof(NearInteractionGrabbable)) as NearInteractionGrabbable;

            // Other components
            codeBlockSnap = gameObject.AddComponent(typeof(CodeBlockSnap)) as CodeBlockSnap;
            snapColliders = GetComponentInChildren<SnapColliders>();

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

        public IEnumerable GetAllAttachedCodeBlocks() {
            if (IsDataCodeBlock()) {
                yield break;
            }
            if (myInstruction.GetNextInstruction() != null) {
                yield return myInstruction.GetNextInstruction().GetCodeBlock();
            }
            for (int i = 0; i < argumentCodeBlocks.Count; ++i) {
                if (argumentCodeBlocks[i] != null) {
                    yield return argumentCodeBlocks[i];
                }
            }
        }

        public SnapColliders GetSnapColliders() {
            if (snapColliders == null) {
                snapColliders = GetComponentInChildren<SnapColliders>();
            }
            return snapColliders;
        }

        public IArgument GetArgumentAt(int position) {
            return myInstruction.GetArgumentAt(position);
        }

        public void SetValueOfData(IDataType dTIn) {
            Assert.IsTrue(IsDataCodeBlock());
            myData.SetValue(dTIn.GetValue());
            UpdateText();
        }

        public void SetNextCodeBlock(CodeBlock newInstructionCodeBlock, Vector3 newLocalPosition) {
            Assert.IsTrue(IsInstructionCodeBlock());
            RemoveNextCodeBlock();
            newInstructionCodeBlock.RemoveFromParentBlock();
            AddNewNextCodeBlock(newInstructionCodeBlock, newLocalPosition);
        }

        public void SetArgumentBlockAt(CodeBlock newArgumentCodeBlock, int argPosition, Vector3 newLocalPosition) {
            Assert.IsTrue(IsInstructionCodeBlock());
            RemoveArgumentAt(argPosition);
            newArgumentCodeBlock.RemoveFromParentBlock();
            AddNewArgumentAt(newArgumentCodeBlock, argPosition, newLocalPosition);
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
                    parentCodeBlock.SetNextCodeBlock(null, Vector3.zero);
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
        private void AddNewNextCodeBlock(CodeBlock newCodeBlock, Vector3 newLocalPosition) {
            nextCodeBlock = newCodeBlock;
            if (newCodeBlock) {
                newCodeBlock.transform.SetParent(transform);
                newCodeBlock.transform.localPosition = newLocalPosition; // TODO: once arg placing is done, update this for better placement
            }
            SetNextInstruction(newCodeBlock.GetInstruction());
        }

        private void AddNewArgumentAt(CodeBlock newArgumentCodeBlock, int position, Vector3 newLocalPosition) {
            // need to update instruction arguments
            argumentCodeBlocks[position] = newArgumentCodeBlock;
            if (newArgumentCodeBlock) {
                newArgumentCodeBlock.transform.SetParent(transform);
                newArgumentCodeBlock.transform.localPosition = newLocalPosition;
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
            if (nextCodeBlock != null) {
                nextCodeBlock.transform.localPosition = new Vector3(1.05f, 1.05f, 0); // TODO: This Placement
                nextCodeBlock.transform.SetParent(CodeBlockManager.instance.transform);
                nextCodeBlock = null;
            }
        }

        private void RemoveArgumentAt(int position) {
            SetArgumentAt(null, position);
            if (argumentCodeBlocks[position] != null) {
                argumentCodeBlocks[position].transform.localPosition = new Vector3(2.05f, 1.05f, 0); // TODO: This Placement
                argumentCodeBlocks[position].transform.SetParent(CodeBlockManager.instance.transform);
                argumentCodeBlocks[position] = null;
            }
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