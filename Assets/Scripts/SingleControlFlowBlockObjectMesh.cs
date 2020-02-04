using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SingleControlFlowBlockObjectMesh : CodeBlockObjectMesh {
        Transform top, side, bottom;
        MeshOutline topOutline, sideOutline, bottomOutline;
        List<MeshOutline> outlines;

        private void Awake() {
            top = transform.GetChild(0);
            side = transform.GetChild(1);
            bottom = transform.GetChild(2);
        }

        public override void AlertInstructionChanged() {
            ResizeMeshes();
        }

        public override Transform GetExitInstructionParentTransform() {
            return bottom;
        }

        public override void ToggleOutline(bool on) {
            if (on) {
                foreach (MeshOutline mo in GetMeshOutlines()) {
                    ToggleOn(mo);
                }
            }
            else {
                foreach (MeshOutline mo in GetMeshOutlines()) {
                    ToggleOff(mo);
                }
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
            ToggleOff(mo);
        }

        private void ToggleOn(MeshOutline mo) {
            mo.OutlineWidth = 0.05f;
        }

        private void ToggleOff(MeshOutline mo) {
            mo.OutlineWidth = 0.00f;
        }

        private List<MeshOutline> GetMeshOutlines() {
            if (outlines == null) {
                outlines = new List<MeshOutline>() { topOutline, sideOutline, bottomOutline };
            }
            return outlines;
        }
    }
}
