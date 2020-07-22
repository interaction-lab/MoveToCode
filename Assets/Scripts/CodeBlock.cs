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
        GameObject codeBlockTextGameObject;
        CloneOnDrag dragScript;
        bool isMenuBlock;

        // Abstract Methods
        protected abstract void SetMyBlockInternalArg();

        // Start Up
        private void Awake() {
            // MRTK components to add
            manipHandler = gameObject.AddComponent<ManipulationHandler>();
            manipHandler.TwoHandedManipulationType = ManipulationHandler.TwoHandedManipulation.MoveRotate;

            // Other components
            codeBlockSnap = gameObject.AddComponent<CodeBlockSnap>();
            snapColliders = GetComponentInChildren<SnapColliderGroup>();

            // Setup
            SetMyBlockInternalArg();
            CodeBlockManager.instance.RegisterCodeBlock(this);

            // ArgListManager set up
            codeBlockArgumentList = gameObject.AddComponent<CodeBlockArgumentList>();
            codeBlockArgumentList.SetUp(this);

            if (GetComponent<ManipulationLogger>() == null) {
                gameObject.AddComponent<ManipulationLogger>();
            }

            dragScript = gameObject.AddComponent<CloneOnDrag>();

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

        public Material GetCodeBlockMaterial() {
            return GetCodeBlockObjectMesh().transform.GetChild(0).GetComponent<MeshRenderer>().material;
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
            return -1;
        }

        // CodeBlockArgumentList relay functions
        public void SetArgumentBlockAt(CodeBlock newArgumentCodeBlock, int argPosition, bool humanDidIt) {
            codeBlockArgumentList.SetArgCodeBlockAt(newArgumentCodeBlock, argPosition, humanDidIt);
            UpdateText();
        }

        public List<CodeBlock> GetArgumentListAsCodeBlocks() {
            return codeBlockArgumentList.GetArgListCodeBlocks();
        }

        public List<IArgument> GetArgumentListAsIArgs() {
            return codeBlockArgumentList.GetArgListAsIArguments();
        }

        public List<Type> GetArgCompatabilityAt(int pos) {
            return (GetMyInternalIArgument() as IArgument).GetArgCompatibilityAtPos(pos);
        }

        public CodeBlock GetArgAsCodeBlockAt(int pos) {
            return codeBlockArgumentList.GetArgAsCodeBlockAt(pos);
        }

        public IArgument GetArgAsIArgumentAt(int pos) {
            return codeBlockArgumentList.GetArgAsIArgumentAt(pos);
        }

        public void ResetInstructionInternalState() {
            myBlockInternalArg.ResestInternalState();
        }

        // Relay to object mesh
        public void ToggleOutline(bool on) {
            GetCodeBlockObjectMesh().ToggleOutline(on);
        }

        public void ToggleColliders(bool on) {
            GetCodeBlockObjectMesh().ToggleColliders(on);
        }

        public void RemoveFromParentBlock(bool humanDidIt) {
            CodeBlock parentCodeBlock = FindParentCodeBlock();
            if (parentCodeBlock != null) {
                parentCodeBlock.SetArgumentBlockAt(null, parentCodeBlock.GetPositionOfArgument(GetMyInternalIArgument()), humanDidIt);
            }
        }

        public void SetIsMenuBlock(bool option) {
            isMenuBlock = option;
        }

        public bool GetIsMenuBlock() {
            return isMenuBlock;
        }

        // Private Helpers
        // If you find yourself making these public, 
        // then you should reconsider what you are doing
        public CodeBlock FindParentCodeBlock() {
            Transform upRunner = transform;
            while (upRunner?.GetComponent<CodeBlockManager>() == null) {
                upRunner = upRunner?.parent;
                if (upRunner?.GetComponent<CodeBlock>()) {
                    return upRunner.GetComponent<CodeBlock>();
                }
            }
            return null;
        }

        // This is needed to wait for the gameobject to spawn
        private IEnumerator UpdateTextNextFrame() {
            yield return new WaitForEndOfFrame();
            UpdateText();
        }

        private void UpdateText() {
            if (textMesh == null) {
                textMesh = codeBlockTextGameObject?.GetComponent<TextMeshPro>();
            }
            if (textMesh == null) {
                codeBlockTextGameObject = Instantiate(
                    Resources.Load<GameObject>(ResourcePathConstants.CodeBlockTextPrefab), transform) as GameObject;
                StartCoroutine(UpdateTextNextFrame());
            }
            else {
                textMesh.SetText(ToString());
                // Forces text update
                textMesh.enabled = false;
                textMesh.enabled = true;
            }
        }

        public override string ToString() {
            return myBlockInternalArg.ToString();
        }

        private void OnEnable() {
            CodeBlockManager.instance.RegisterCodeBlock(this);
        }

        private void OnDestroy() {
            if (CodeBlockManager.instance != null && CodeBlockManager.instance.isActiveAndEnabled) {
                CodeBlockManager.instance.DeregisterCodeBlock(this);
            }
        }

        private void OnDisable() {
            if (CodeBlockManager.instance != null && CodeBlockManager.instance.isActiveAndEnabled) {
                CodeBlockManager.instance.DeregisterCodeBlock(this);
            }
        }
    }
}