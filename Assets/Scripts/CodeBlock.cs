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

            if (GetComponent<ManipulationLogger>() == null) {
                gameObject.AddComponent<ManipulationLogger>();
            }

            dragScript = gameObject.AddComponent<CloneOnDrag>();

            UpdateText();
        }


        // Public Methods      
        public IArgument GetMyIArgument() {
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
        public SnapColliderGroup GetSnapColliders() {
            if (snapColliders == null) {
                snapColliders = GetComponentInChildren<SnapColliderGroup>();
            }
            return snapColliders;
        }



        public HashSet<Type> GetArgCompatibility(SNAPCOLTYPEDESCRIPTION argDescription) {
            return GetMyIArgument().GetArgCompatibility(argDescription);
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

        public void RemoveFromParentSnapCollider(bool humanDidIt) {
            SnapCollider parentSnapCollider = transform.parent?.GetComponent<SnapCollider>();
            if (parentSnapCollider != null) {
                parentSnapCollider.SetMyCodeBlockArg(null);
                // TODO: probably needs a log
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