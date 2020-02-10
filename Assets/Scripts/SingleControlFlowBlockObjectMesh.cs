using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SingleControlFlowBlockObjectMesh : CodeBlockObjectMesh {
        Transform top, side, bottom;

        // private methods

        private int FindChainSize() {
            return transform.parent.GetComponent<CodeBlock>().FindChainSize();
        }

        private int GetMyVerticalSize() {
            return transform.parent.GetComponent<CodeBlock>().GetBlockVerticalSize();
        }

        private void ResizeMeshes() {
            int chainSize = FindChainSize(); // Also need trailing size of things within, make this within chain

            Vector3 scaler = side.localScale;
            scaler.y = chainSize + GetMyVerticalSize();
            side.localScale = scaler;

            scaler = side.localPosition;
            scaler.y = -(side.localScale.y - 1) / 2;
            side.localPosition = scaler;

            // need to move down bottom also
            scaler = bottom.localPosition;
            scaler.y = -chainSize - 2;
            bottom.localPosition = scaler;
        }

        private void AddOutlineToObject(GameObject ob, ref MeshOutline mo, Material outlineMaterial) {
            mo = ob.AddComponent(typeof(MeshOutline)) as MeshOutline;
            mo.OutlineMaterial = outlineMaterial;
            mo.OutlineWidth = 0.05f;
            mo.enabled = false;
        }

        private List<MeshOutline> GetMeshOutlines() {
            if (outlines == null) {
                outlines = new List<MeshOutline>() { topOutline, sideOutline, bottomOutline };
            }
            return outlines;
        }

        public override void SetUpObjectMesh() {
            throw new System.NotImplementedException();
        }

        public override void SetUpMeshOutlineList() {
            throw new System.NotImplementedException();
        }

        public override void SnapArgAtPosition(CodeBlock cbIn, int pos) {
            throw new System.NotImplementedException();
        }

        public override float GetBlockVerticalSize() {
            throw new System.NotImplementedException();
        }

        public override float GetBlockHorizontalSize() {
            throw new System.NotImplementedException();
        }

        public override void ResizeInstruction() {
            throw new System.NotImplementedException();
        }

        // chain size
    }
}
