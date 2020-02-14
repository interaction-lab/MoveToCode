using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public abstract class CodeBlockObjectMesh : MonoBehaviour {
        protected CodeBlock myCodeBlock;
        protected List<MeshOutline> meshOutlineList;
        protected static Material outlineMaterial;
        protected SnapColliderGroup snapColliderGroup;

        // Set up
        void Awake() {
            SetUpObject();
            SetUpMeshOutlineList();
            ConfigureOutlines();
        }

        public abstract void SetUpObject();
        public abstract void SetUpMeshOutlineList();
        public void ConfigureOutlines() {
            foreach (MeshOutline mo in meshOutlineList) {
                mo.OutlineMaterial = GetOutlineMaterial();
                mo.OutlineWidth = 0.05f;
                mo.enabled = false;
                Rigidbody rigidBody = mo.gameObject.AddComponent<Rigidbody>();
                rigidBody.isKinematic = true;
                rigidBody.useGravity = false;
            }
        }

        // Action change
        public CodeBlock GetMyCodeBlock() {
            if (myCodeBlock == null) {
                myCodeBlock = transform.parent.GetComponent<CodeBlock>(); // object mesh should always be right below codeblock in heirarchy
            }
            return myCodeBlock;
        }
        public SnapColliderGroup GetSnapColliderGroup() {
            if (snapColliderGroup == null) {
                snapColliderGroup = GetComponent<SnapColliderGroup>();
            }
            return snapColliderGroup;
        }

        public void AlertInstructionSizeChanged() {
            ResizeObjectMesh();
            GetMyCodeBlock().AlertParentCodeBlockOfSizeChange();
        }

        // This is for resizing needs
        public abstract float GetBlockVerticalSize();
        public abstract float GetBlockHorizontalSize();
        public abstract Vector3 GetCenterPosition();
        public abstract void ResizeObjectMesh();

        protected float FindChainSize(IArgument aIn) {
            StandAloneInstruction sai = aIn as StandAloneInstruction;
            if (sai != null) {
                return sai.GetCodeBlock().GetCodeBlockObjectMesh().GetBlockVerticalSize(); // todo maybe make this cleaner?
            }
            return 0;
        }

        // Mesh outline
        public void ToggleOutline(bool on) {
            if (meshOutlineList == null) // TODO: this is hack, fix for reset
                return;
            foreach (MeshOutline mo in meshOutlineList) {
                mo.enabled = on;
            }
        }

        public void ToggleColliders(bool on) {
            if (meshOutlineList == null) // TODO: this is hack, fix for reset
                return;
            foreach (MeshOutline mo in meshOutlineList) {
                mo.gameObject.GetComponent<Collider>().enabled = on;
            }
        }

        // Getters
        public static Material GetOutlineMaterial() {
            if (outlineMaterial == null) {
                outlineMaterial = Resources.Load<Material>(ResourcePathConstants.OutlineCodeBlockMaterial);
            }
            return outlineMaterial;
        }
    }
}