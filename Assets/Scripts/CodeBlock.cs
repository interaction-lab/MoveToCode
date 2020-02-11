using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;


namespace MoveToCode {
    public abstract class CodeBlock : MonoBehaviour {
        protected IArgument myBlockInternalArg;
        TextMeshPro textMesh;
        ManipulationHandler manipHandler;
        CodeBlockObjectMesh codeBlockObjectMesh;
        SnapColliderGroup snapColliders;
        CodeBlockArgumentList codeBlockArgumentList;
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

        public CodeBlockSnap GetCodeBlockSnap() {
            return codeBlockSnap;
        }

        public CodeBlockObjectMesh GetCodeBlockObjectMesh() {
            if (codeBlockObjectMesh == null) {
                codeBlockObjectMesh = GetComponentInChildren<CodeBlockObjectMesh>();
            }
            return codeBlockObjectMesh;
        }

        // this should be from object mesh
        //public IEnumerable GetAllAttachedCodeBlocks() { // this should just be from the object mesh
        public SnapColliderGroup GetSnapColliders() {
            if (snapColliders == null) {
                snapColliders = GetComponentInChildren<SnapColliderGroup>();
            }
            return snapColliders;
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

        // CodeBlockArgumentList relay functions
        public void SetArgumentBlockAt(CodeBlock newArgumentCodeBlock, int argPosition, Vector3 newLocalPosition) {
            codeBlockArgumentList.SetArgCodeBlockAt(newArgumentCodeBlock, argPosition, newLocalPosition);
            UpdateText();
        }

        public List<CodeBlock> GetArgumentListAsCodeBlocks() {
            return codeBlockArgumentList.GetArgListCodeBlocks();
        }

        public List<IArgument> GetArgumentListAsIArgs() {
            return codeBlockArgumentList.GetArgListAsIArguments();
        }

        public List<Type> GetArgCompatabilityAt(int pos) {
            return (GetMyInternalIArgument() as Instruction).GetArgCompatibilityAtPos(pos);
        }

        public IArgument GetArgAsIArgumentAt(int pos) {
            return codeBlockArgumentList.GetArgAsIArgumentAt(pos);
        }

        // Note: This is slightly inefficienct approach but makes it so you don't have to keep track of as much
        public void RemoveFromParentBlock() {
            CodeBlock parentCodeBlock = FindParentCodeBlock();
            if (parentCodeBlock != null) {
                parentCodeBlock.SetArgumentBlockAt(null, parentCodeBlock.GetPositionOfArgument(GetInternalIArg()), Vector3.zero);
            }
            parentCodeBlock.UpdateText();
        }

        public void AlertParentCodeBlockOfSizeChange() {
            CodeBlock parentCodeBlock = FindParentCodeBlock();
            if (parentCodeBlock != null) {
                parentCodeBlock.GetCodeBlockObjectMesh().AlertInstructionSizeChanged();
            }
        }

        // Private Helpers
        // If you find yourself making these public, 
        // then you should reconsider what you are doing
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

        // CONTROL FLOW SIZES, these should be moved out to object meshes
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