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
        protected IArgument myBlockInternalArg;
        CodeBlock nextCodeBlock;
        List<CodeBlock> argumentCodeBlocks;
        TextMeshPro textMesh;
        ManipulationHandler manipHandler;
        NearInteractionGrabbable nearInteractionGrabbable;
        CodeBlockSnap codeBlockSnap;
        Rigidbody rigidBody;
        SnapColliderGroup snapColliders;
        protected BaseMeshOutline meshOutline;
        protected static Material outlineMaterial;

        // Abstract Methods
        protected abstract void SetMyBlockInternalArg();

        // Virtual methods, made for control blocks really
        public virtual int GetBlockVerticalSize() {
            return 1;
        }

        public virtual Vector3 GetSnapToParentPosition() {
            return Vector3.zero;
        }

        public void ResetInstructionInternalState() {
            myBlockInternalArg.ResestInternalState();
        }

        // MeshOutlines
        protected virtual void SetupMeshOutline() {
            if (outlineMaterial == null) {
                outlineMaterial = Resources.Load<Material>(ResourcePathConstants.OutlineCodeBlockMaterial) as Material;
            }
            if (meshOutline == null) {
                meshOutline = gameObject.AddComponent(typeof(MeshOutline)) as MeshOutline;
                meshOutline.OutlineMaterial = outlineMaterial;
                meshOutline.OutlineWidth = 0.05f;
            }
        }

        protected BaseMeshOutline GetMeshOutline() {
            if (meshOutline == null) {
                SetupMeshOutline();
            }
            return meshOutline;
        }

        public virtual void ToggleOutline(bool on) {
            if (on) {
                GetMeshOutline().OutlineWidth = 0.05f;
            }
            else {
                GetMeshOutline().OutlineWidth = 0.0f;
            }
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
            nearInteractionGrabbable = gameObject.AddComponent(typeof(NearInteractionGrabbable)) as NearInteractionGrabbable;

            // Other components
            codeBlockSnap = gameObject.AddComponent(typeof(CodeBlockSnap)) as CodeBlockSnap;
            snapColliders = GetComponentInChildren<SnapColliderGroup>();

            // Setup
            argumentCodeBlocks = new List<CodeBlock>();
            SetMyBlockInternalArg();
            myBlockInternalArg.SetCodeBlock(this);
            if (IsInstructionCodeBlock()) {
                argumentCodeBlocks.Resize(GetMyInstruction().GetNumArguments());
            }

            CodeBlockManager.instance.RegisterCodeBlock(this);

            UpdateText();
        }

        // Public Methods
        public bool IsInstructionCodeBlock() {
            return (this as InstructionCodeBlock) != null;
        }
        public bool IsDataCodeBlock() {
            return (this as DataCodeBlock) != null;
        }
        public bool IsControlFlowCodeBlock() {
            return (this as ControlFlowCodeBlock) != null;
        }

        public Instruction GetMyInstruction() {
            return myBlockInternalArg as Instruction;
        }
        public IDataType GetMyData() {
            return myBlockInternalArg as IDataType;
        }
        public IArgument GetMyInternalIArgument() {
            return myBlockInternalArg;
        }

        public IEnumerable GetAllAttachedCodeBlocks() {
            if (IsDataCodeBlock()) {
                yield break;
            }
            if (GetMyInstruction().GetNextInstruction() != null) {
                yield return GetMyInstruction().GetNextInstruction().GetCodeBlock();
            }
            for (int i = 0; i < argumentCodeBlocks.Count; ++i) {
                if (argumentCodeBlocks[i] != null) {
                    yield return argumentCodeBlocks[i];
                }
            }
        }

        public SnapColliderGroup GetSnapColliders() {
            if (snapColliders == null) {
                snapColliders = GetComponentInChildren<SnapColliderGroup>();
            }
            return snapColliders;
        }

        public IArgument GetArgumentAt(int position) {
            return GetMyInstruction().GetArgumentAt(position);
        }

        public void SetValueOfData(IDataType dTIn) {
            Assert.IsTrue(IsDataCodeBlock());
            GetMyData().SetValue(dTIn.GetValue());
            UpdateText();
        }

        public void SetNextCodeBlock(CodeBlock newInstructionCodeBlock, Vector3 newLocalPosition) {
            Assert.IsTrue(IsInstructionCodeBlock());
            RemoveNextCodeBlock();
            newInstructionCodeBlock?.RemoveFromParentBlock();
            AddNewNextCodeBlock(newInstructionCodeBlock, newLocalPosition);
        }

        public void SetArgumentBlockAt(CodeBlock newArgumentCodeBlock, int argPosition, Vector3 newLocalPosition) {
            Assert.IsTrue(IsInstructionCodeBlock());
            RemoveArgumentAt(argPosition);
            newArgumentCodeBlock?.RemoveFromParentBlock();
            AddNewArgumentAt(newArgumentCodeBlock, argPosition, newLocalPosition);
            UpdateText();
        }

        public bool IsMyNextInstruction(Instruction iIn) {
            return GetMyInstruction()?.GetNextInstruction() == iIn;
        }

        public int GetPositionOfArgument(IArgument iArgIn) {
            int index = 0;
            foreach (CodeBlock codeBlock in argumentCodeBlocks) {
                if (codeBlock?.GetArgumentValueOfCodeBlock() == iArgIn) {
                    return index;
                }
                ++index;
            }
            Assert.IsTrue(false); // Should be able to find argument, must call IsMyNextInstruction first
            return -1; // Will never get here, put so VS stops complaining
        }

        public int FindChainSize() {
            int size = 0;
            Instruction runner = GetMyInstruction().GetNextInstruction();
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
        // TODO add in endifinstructions to this
        public void RemoveFromParentBlock() {
            CodeBlock parentCodeBlock = transform.parent?.GetComponent<CodeBlock>();
            ControlFlowCodeBlock parentAsControlFlowBlock = parentCodeBlock as ControlFlowCodeBlock;
            if (parentCodeBlock != null) {
                if (IsInstructionCodeBlock() && parentCodeBlock.IsMyNextInstruction(GetMyInstruction())) {
                    parentCodeBlock.SetNextCodeBlock(null, Vector3.zero);
                }
                else if (parentAsControlFlowBlock != null &&
                    parentAsControlFlowBlock.IsMyExitInstruction(GetMyInstruction())) {
                    parentAsControlFlowBlock.SetExitCodeBlock(null, Vector3.zero);
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
                newCodeBlock.transform.SnapToParent(transform, newLocalPosition);
            }
            SetNextInstruction(newCodeBlock?.GetMyInstruction());
        }

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