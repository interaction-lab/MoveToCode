using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public abstract class CodeBlockObjectMesh : MonoBehaviour {
        protected CodeBlock myCodeBlock;
        protected List<MeshOutline> meshOutlineList;
        protected static Material outlineMaterial;

        // Set up
        public CodeBlockObjectMesh() {
            SetUpObjectMesh();
            SetUpMeshOutlineList();
        }
        public abstract void SetUpObjectMesh();
        public abstract void SetUpMeshOutlineList();


        // Action change
        public abstract void SnapArgAtPosition(CodeBlock cbIn, int pos);
        public CodeBlock GetMyCodeBlock() {
            if (myCodeBlock == null) {
                myCodeBlock = transform.parent.GetComponent<CodeBlock>(); // object mesh should always be right below codeblock in heirarchy
            }
            return myCodeBlock;
        }
        public void AlertInstructionSizeChanged() {
            ResizeInstruction();
            GetMyCodeBlock().AlertParentCodeBlockOfSizeChange();
        }

        // This is for resizing needs
        public abstract float GetBlockVerticalSize();
        public abstract float GetBlockHorizontalSize();
        public abstract void ResizeInstruction();

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