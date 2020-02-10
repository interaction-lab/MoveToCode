using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SingleRightArgObjectMesh : CodeBlockObjectMesh {
        public override void AlertInstructionSizeChanged() {
            ResizeInstruction();
            GetMyCodeBlock().
        }

        public override int GetBlockHorizontalSize() {
            throw new System.NotImplementedException();
        }

        public override int GetBlockVerticalSize() {
            throw new System.NotImplementedException();
        }

        public override bool IsOutlineSetUp() {
            throw new System.NotImplementedException();
        }

        public override void ResizeInstruction() {
            throw new System.NotImplementedException();
        }

        public override void SetUpMeshOutline(Material outlineMaterial) {
            throw new System.NotImplementedException();
        }

        public override void SnapArgAtPosition(CodeBlock cbIn, int pos) {
            throw new System.NotImplementedException();
        }

        public override void ToggleOutline(bool on) {
            throw new System.NotImplementedException();
        }
    }
}
}