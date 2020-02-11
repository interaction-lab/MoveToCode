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
        public abstract void ResizeObjectMesh();

        // Mesh outline
        public void ToggleOutline(bool on) {
            foreach (MeshOutline mo in meshOutlineList) {
                mo.enabled = on;
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