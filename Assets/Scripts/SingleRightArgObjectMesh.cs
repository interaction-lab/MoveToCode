using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SingleRightArgObjectMesh : CodeBlockObjectMesh {
        Transform top, argRight;
        Vector3 origScaleArgRight;
        Vector3 origPositionArgRight;

        public override void SetUpObject() {
            top = transform.GetChild(0);
            argRight = transform.GetChild(1);
            origScaleArgRight = argRight.localScale;
            origPositionArgRight = argRight.localPosition;
        }
        public override void SetUpMeshOutlineList() {
            meshOutlineList = new List<MeshOutline>() {
                top.gameObject.AddComponent<MeshOutline>(),
                argRight.gameObject.AddComponent<MeshOutline>()
                };
        }

        public override float GetBlockHorizontalSize() {
            return top.localScale.x + argRight.localScale.x;
        }

        public override float GetBlockVerticalSize() {
            return top.localScale.y + FindChainSize(GetMyCodeBlock().GetArgAsIArgumentAt(0));
        }

        public override Vector3 GetCenterPosition() {
            return Vector3.zero; // todo later
        }

        protected override void ResizeObjectMesh() {
            // need to resize arg right based upon horizontal size of arg
            Vector3 rescale = origScaleArgRight;
            Vector3 reposition = origPositionArgRight;
            float? horizontalSize = GetMyCodeBlock().GetArgAsCodeBlockAt(1)?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize;
                reposition.x = reposition.x + (rescale.x - 0.5f) / 2.0f;
            }
            argRight.localPosition = reposition;
            argRight.localScale = rescale;
        }
    }
}
