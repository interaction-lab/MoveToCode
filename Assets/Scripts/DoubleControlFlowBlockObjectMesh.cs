using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class DoubleControlFlowBlockObjectMesh : CodeBlockObjectMesh {
        Transform top, sideTop, middle, sideBottom, bottom;
#pragma warning disable 0649 // disable outline warning
        MeshOutline topOutline, sideTopOutline, middleOutline, sideBottomOutline, bottomOutline;
        List<MeshOutline> outlines;
        List<Transform> transforms;

        private void Awake() {
            top = transform.GetChild(0);
            sideTop = transform.GetChild(1);
            middle = transform.GetChild(2);
            sideBottom = transform.GetChild(3);
            bottom = transform.GetChild(4);
        }

        public override void AlertInstructionSizeChanged() {
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
            AddOutlineToObject(sideTop.gameObject, ref sideTopOutline, outlineMaterial);
            AddOutlineToObject(middle.gameObject, ref middleOutline, outlineMaterial);
            AddOutlineToObject(sideBottom.gameObject, ref sideBottomOutline, outlineMaterial);
            AddOutlineToObject(bottom.gameObject, ref bottomOutline, outlineMaterial);
        }

        public override bool IsOutlineSetUp() {
            return topOutline != null;
        }

        // private methods
        private int FindChainSizeTop() {
            return transform.parent.GetComponent<CodeBlock>().FindChainSize();
        }
        private int FindChainSizeBot() { // Jank fix for else
            return transform.parent.GetComponent<CodeBlock>().FindChainSizeOfArgIndex(1);
        }

        private int GetMyVerticalSize() {
            return transform.parent.GetComponent<CodeBlock>().GetBlockVerticalSize();
        }

        private void ResizeMeshes() {
            ResizeTop();
            ResizeBot();

        }
        private void ResizeTop() {
            int chainSize = FindChainSizeTop();

            Vector3 scaler = sideTop.localScale;
            scaler.y = chainSize + GetMyVerticalSize();
            sideTop.localScale = scaler;

            scaler = sideTop.localPosition;
            scaler.y = -(sideTop.localScale.y - 1) / 2;
            sideTop.localPosition = scaler;

            // need to move down bottom also
            scaler = middle.localPosition;
            scaler.y = -chainSize - 2;
            middle.localPosition = scaler;
        }

        private void ResizeBot() {
            int chainSize = FindChainSizeBot();

            Vector3 scaler = sideBottom.localScale;
            scaler.y = chainSize + GetMyVerticalSize();
            sideBottom.localScale = scaler;

            scaler = sideBottom.localPosition;
            scaler.y = -(sideBottom.localScale.y - 1) / 2; // here 
            sideBottom.localPosition = scaler;

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
                outlines = new List<MeshOutline>() { topOutline, sideTopOutline, middleOutline, sideBottomOutline, bottomOutline };
            }
            return outlines;
        }

        private List<Transform> GetObjectTransforms() {
            if (transforms == null) {
                transforms = new List<Transform>() { top, sideTop, middle, sideBottom, bottom };
            }
            return transforms;
        }
    }
}
