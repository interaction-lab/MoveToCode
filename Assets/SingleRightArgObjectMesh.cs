using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SingleRightArgObjectMesh : CodeBlockObjectMesh {

        GameObject top;
        MeshOutline topOutline;

        // do we want to keep an internal size of this... or just recalc everyt time.. probably recalc
        public override float GetBlockHorizontalSize() {
            return transform.localScale.y;
        }

        public override float GetBlockVerticalSize() {
            return transform.localScale.y;
        }

        public override void ResizeInstruction() {
            return;//curenntly does nothing
        }

        public override void SetUpMeshOutlineList() {
            meshOutlineList = new List<MeshOutline>() { top.gameObject.AddComponent<MeshOutline>() };
        }

        public override void SetUpObjectMesh() {
            throw new System.NotImplementedException();
        }

        public override void SnapArgAtPosition(CodeBlock cbIn, int pos) {
            throw new System.NotImplementedException();
        }
    }
}
