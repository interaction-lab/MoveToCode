using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SingleControlFlowBlockObjectMesh : CodeBlockObjectMesh {
        Transform top, side, bottom;
#pragma warning disable 0649 // disable outline warning
        MeshOutline topOutline, sideOutline, bottomOutline;
        List<MeshOutline> outlines;

        private void Awake() {
            top = transform.GetChild(0);
            side = transform.GetChild(1);
            bottom = transform.GetChild(2);
            topOutline = sideOutline = bottomOutline = null;
        }

        public override void AlertInstructionChanged() {
            ResizeMeshes();
        }

        public override Transform GetExitInstructionParentTransform() {
            return bottom;
        }

        public override void ToggleOutline(bool on) {
            foreach (MeshOutline mo in GetMeshOutlines()) {
                mo.enabled = on;
            }
        }

        public override void SetUpMeshOutline(Material outlineMaterial) {
            AddOutlineToObject(top.gameObject, ref topOutline, outlineMaterial);
            AddOutlineToObject(side.gameObject, ref sideOutline, outlineMaterial);
            AddOutlineToObject(bottom.gameObject, ref bottomOutline, outlineMaterial);
        }

        public override bool IsOutlineSetUp() {
            return topOutline != null;
        }

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
    }
}
