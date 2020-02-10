using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;


namespace MoveToCode {
    public abstract class CodeBlock : MonoBehaviour {
        // Defintely keep
        protected IArgument myBlockInternalArg;
        TextMeshPro textMesh;
        ManipulationHandler manipHandler;
        CodeBlockObjectMesh codeBlockObjectMesh;
        SnapColliderGroup snapColliders;
        CodeBlockArgumentList codeBlockArgumentList;

        // IDK yet
        CodeBlockSnap codeBlockSnap;
        Rigidbody rigidBody;

        // Abstract Methods
        protected abstract void SetMyBlockInternalArg();

        // Virtual methods, made for control blocks really
        // this should really be handled by object mesh
        public virtual int GetBlockVerticalSize() {
            return 1;
        }

        public virtual Vector3 GetSnapToParentPosition() {
            return Vector3.zero;
        }

        public void ResetInstructionInternalState() {
            myBlockInternalArg.ResestInternalState();
        }

        // Start Up
        private void Awake() {
            // Set up collision
            rigidBody = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rigidBody.isKinematic = true;
            rigidBody.useGravity = false;
            GetComponent<Collider>().isTrigger = true;

            // MRTK components to add
            manipHandler = gameObject.AddComponent(typeof(ManipulationHandler)) as ManipulationHandler;

            // Other components
            codeBlockSnap = gameObject.AddComponent(typeof(CodeBlockSnap)) as CodeBlockSnap;
            snapColliders = GetComponentInChildren<SnapColliderGroup>();

            // Setup
            SetMyBlockInternalArg();
            CodeBlockManager.instance.RegisterCodeBlock(this);

            // ArgListManager set up
            codeBlockArgumentList = gameObject.AddComponent<CodeBlockArgumentList>();
            codeBlockArgumentList.SetUp(this);

            UpdateText();
        }

        // Public Methods      
        public IArgument GetMyInternalIArgument() {
            return myBlockInternalArg;
        }

        // this should be from object mesh
        //public IEnumerable GetAllAttachedCodeBlocks() { // this should just be from the object mesh
        public SnapColliderGroup GetSnapColliders() {
            if (snapColliders == null) {
                snapColliders = GetComponentInChildren<SnapColliderGroup>();
            }
            return snapColliders;
        }

        public void SetArgumentBlockAt(CodeBlock newArgumentCodeBlock, int argPosition, Vector3 newLocalPosition) {
            codeBlockArgumentList.SetArgCodeBlockAt(newArgumentCodeBlock, argPosition, newLocalPosition);
            UpdateText();
        }

        public int GetPositionOfArgument(IArgument iArgIn) {
            int index = 0;
            foreach (IArgument ia in codeBlockArgumentList.GetArgListAsIArguments()) {
                if (ia == iArgIn) {
                    return index;
                }
                ++index;
            }
            Assert.IsTrue(false); // Should be able to find argument, must call IsMyNextInstruction first
            return -1; // Will never get here, put so VS stops complaining
        }


        // Chain size
        public int FindChainSize() {
            return FindChainSize(this);
        }

        public int FindChainSizeOfArgIndex(int indexIn) {
            return FindChainSize(argumentCodeBlocks[indexIn]) + GetBlockVerticalSize();
        }

        public int FindChainSize(CodeBlock cbIn) {
            if (cbIn == null) {
                return 0;
            }
            int size = 0;
            Instruction runner = cbIn.GetMyInstruction().GetNextInstruction();
            while (runner != null) {
                size += runner.GetCodeBlock().GetBlockVerticalSize();
                ControlFlowInstruction cfi = runner as ControlFlowInstruction;
                if (cfi != null) { // this is to deal with big chains of control flow blocks changing at once
                    size += runner.GetCodeBlock().FindChainSize();
                    runner = cfi.GetExitInstruction();
                }
                else {
                    runner = runner.GetNextInstruction();
                }
            }
            return size;
        }



        // Note: This is slightly inefficienct approach but makes it so you don't have to keep track of as much

        public void RemoveFromParentBlock() {
            CodeBlock parentCodeBlock = FindParentCodeBlock();
            if (parentCodeBlock != null) {
                parentCodeBlock.RemoveArgumentAt(
     parentCodeBlock.GetPositionOfArgument(
         GetArgumentValueOfCodeBlock()));
            }
            parentCodeBlock.UpdateText();
        }

        // Private Helpers
        // If you find yourself making these public, 
        // then you should reconsider what you are doing

        private void AddNewArgumentAt(CodeBlock newArgumentCodeBlock, int position, Vector3 newLocalPosition) {
            try {
                GetMyInstruction().SetArgumentAt(newArgumentCodeBlock?.GetArgumentValueOfCodeBlock(), position);
                if (newArgumentCodeBlock) {
                    newArgumentCodeBlock.transform.SnapToParent(transform, newLocalPosition);
                }
                argumentCodeBlocks[position] = newArgumentCodeBlock;
            }
            catch (Exception ex) {
                Debug.LogWarning(ex.ToString());
            }
        }

        private CodeBlock FindParentCodeBlock() {
            Transform upRunner = transform;
            while (upRunner.GetComponent<CodeBlockManager>() == null) {
                upRunner = upRunner.parent;
                if (upRunner.GetComponent<CodeBlock>()) {
                    return upRunner.GetComponent<CodeBlock>();
                }
            }
            return null;
        }

        private void UpdateParentControlFlowSizes() {
            CodeBlock parentBlock = FindParentCodeBlock();
            ControlFlowCodeBlock parentAsControl = parentBlock as ControlFlowCodeBlock;
            if (parentAsControl != null) {
                parentAsControl.UpdateControlFlowSizes();
            }
            else if (parentBlock != null) {
                parentBlock.UpdateParentControlFlowSizes();
            }
        }

        private void UpdateControlFlowSizes() {
            ControlFlowCodeBlock asControl = this as ControlFlowCodeBlock;
            if (asControl != null) {
                asControl.AlertInstructionChanged();
            }
            UpdateParentControlFlowSizes();
        }

        private void SetNextInstruction(Instruction iIn) {
            GetMyInstruction().SetNextInstruction(iIn);
            UpdateControlFlowSizes();
        }

        private void SetArgumentAt(IArgument newArg, int position) {
            GetMyInstruction().SetArgumentAt(newArg, position);
            UpdateControlFlowSizes(); // this is because we are about to make else an argument
        }

        private void RemoveNextCodeBlock() {
            SetNextInstruction(null);
            if (nextCodeBlock != null) {
                nextCodeBlock.transform.localPosition = nextCodeBlock.transform.localPosition + new Vector3(0.25f, 0.25f, 1.25f); // TODO: This Placement
                nextCodeBlock.transform.SetParent(CodeBlockManager.instance.transform);
                nextCodeBlock = null;
            }
        }

        private void RemoveArgumentAt(int position) {
            SetArgumentAt(null, position);
            if (argumentCodeBlocks[position] != null) {
                argumentCodeBlocks[position].transform.localPosition = argumentCodeBlocks[position].transform.localPosition + new Vector3(0.25f, 0.25f, 1.25f); // TODO: This Placement
                argumentCodeBlocks[position].transform.SetParent(CodeBlockManager.instance.transform);
                argumentCodeBlocks[position] = null;
            }
        }

        private IArgument GetArgumentValueOfCodeBlock() {
            return myBlockInternalArg;
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
                GameObject codeBlockTextGameObject = Instantiate(
                    Resources.Load<GameObject>(ResourcePathConstants.CodeBlockTextPrefab), transform) as GameObject;
                //codeBlockTextGameObject.transform.localPosition = -GetSnapToParentPosition() + new Vector3(0, 0, -0.6f);
                StartCoroutine(UpdateTextNextFrame());
            }
            else {
                textMesh.SetText(ToString());
                textMesh.enabled = false;
                textMesh.enabled = true;
                FindParentCodeBlock()?.UpdateText();
            }
        }

        public override string ToString() {
            return myBlockInternalArg.ToString();
        }

        private void OnDestroy() {
            if (CodeBlockManager.instance && CodeBlockManager.instance.isActiveAndEnabled) {
                CodeBlockManager.instance.DeregisterCodeBlock(this);
            }
        }
    }
}