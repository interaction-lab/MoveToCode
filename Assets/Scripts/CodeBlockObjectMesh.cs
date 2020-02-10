using UnityEngine;

namespace MoveToCode {
    public abstract class CodeBlockObjectMesh : MonoBehaviour {
        protected CodeBlock myCodeBlock;

        // Action change
        public abstract void SnapArgAtPosition(CodeBlock cbIn, int pos);
        public CodeBlock GetMyCodeBlock() {
            if (myCodeBlock == null) {
                myCodeBlock = transform.parent.GetComponent<CodeBlock>(); // object mesh should always be right below codeblock in heirarchy
            }
            return myCodeBlock;
        }

        // This is for resizing needs
        public abstract void AlertInstructionSizeChanged();
        public abstract int GetBlockVerticalSize();
        public abstract int GetBlockHorizontalSize();
        public abstract void ResizeInstruction();

        // Mesh outline
        public abstract void SetUpMeshOutline(Material outlineMaterial);
        public abstract void ToggleOutline(bool on);
        public abstract bool IsOutlineSetUp();
    }
}