using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using static Microsoft.MixedReality.Toolkit.UI.ObjectManipulator;

namespace MoveToCode {
    public abstract class CodeBlock : MonoBehaviour {
        protected IArgument myBlockInternalArg;
        TextMeshPro textMesh;
        ManipulationHandler manipHandler;
        CodeBlockObjectMesh codeBlockObjectMesh;
        SnapColliderGroup snapColliders;
        CodeBlockSnap codeBlockSnap;
        GameObject codeBlockTextGameObject;
        CloneOnDrag dragScript;

        bool isMenuBlock;

        // Abstract Methods
        protected abstract void SetMyBlockInternalArg();

        // Start Up
        private void Awake() {
            AddMRTKComponents();
            AddSnapColliderComponents();
            if (myBlockInternalArg == null) {
                SetMyBlockInternalArg();
            }
            CodeBlockManager.instance.RegisterCodeBlock(this);
            SetUpManipulationLogger();
            dragScript = gameObject.AddComponent<CloneOnDrag>(); // TODO: clean this up
            UpdateText();
            // turn alpha of all meshes below this to 230
            codeBlockObjectMesh.SetAlpha(230);
        }



        private void SetUpManipulationLogger() {
            if (GetComponent<ManipulationLogger>() == null) {
                gameObject.AddComponent<ManipulationLogger>();
            }
        }

        private void AddMRTKComponents() {
            manipHandler = gameObject.AddComponent<ManipulationHandler>();
            manipHandler.ManipulationType = ManipulationHandler.HandMovementType.OneHandedOnly;
            manipHandler.AllowFarManipulation = true;
            manipHandler.OneHandRotationModeFar = ManipulationHandler.RotateInOneHandType.FaceAwayFromUser;
            manipHandler.OneHandRotationModeNear = ManipulationHandler.RotateInOneHandType.FaceAwayFromUser;
        }

        private void AddSnapColliderComponents() {
            codeBlockSnap = gameObject.AddComponent<CodeBlockSnap>();
            snapColliders = gameObject.GetComponentInChildrenOnlyDepthOne<SnapColliderGroup>();
        }



        // Public Methods      
        public IArgument GetMyIArgument() {
            if (myBlockInternalArg == null) {
                SetMyBlockInternalArg();
            }
            return myBlockInternalArg;
        }

        public CodeBlockSnap GetCodeBlockSnap() {
            return codeBlockSnap;
        }

        public CodeBlockObjectMesh GetCodeBlockObjectMesh() {
            if (codeBlockObjectMesh == null) {
                codeBlockObjectMesh = gameObject.GetComponentInChildrenOnlyDepthOne<CodeBlockObjectMesh>();
            }
            return codeBlockObjectMesh;
        }

        public Material GetCodeBlockMaterial() {
            return GetCodeBlockObjectMesh().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        }

        // this should be from object mesh
        public SnapColliderGroup GetSnapColliderGroup() {
            if (snapColliders == null) {
                snapColliders = gameObject.GetComponentInChildrenOnlyDepthOne<SnapColliderGroup>();
            }
            return snapColliders;
        }

        public CodeBlock GetSnapColliderCodeBlock(KeyValuePair<Type, int> key) {
            return GetArgumentFromDict(key)?.GetCodeBlock();
        }

        internal Dictionary<string, SnapCollider> GetArgDictAsCodeBlocks() {
            return GetMyIArgument().GetArgToSnapColliderDict();
        }

        public IArgument GetArgumentFromDict(KeyValuePair<Type, int> key) {
            return GetMyIArgument().GetArgument(key);
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

        public SnapCollider GetSnapColliderImAttachedTo() {
            return transform.parent.GetComponentInChildrenOnlyDepthOne<SnapCollider>();
        }
        public void RemoveFromParentSnapCollider(bool humanDidIt) {
            GetSnapColliderImAttachedTo()?.SetCodeBlockArg(null);
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

        public void UpdateText() {
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
                textMesh.ForceTextUpdate();
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