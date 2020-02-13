using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class LeftAndRightArgObjectMesh : CodeBlockObjectMesh {
        Transform top, argLeft, argRight;
        Vector3 origScaleArg;
        Vector3 origPositionArgLeft, origPositionArgRight;

        public override void SetUpObject() {
            top = transform.GetChild(0);
            argLeft = transform.GetChild(1);
            argRight = transform.GetChild(2);
            origScaleArg = argRight.localScale;
            origPositionArgLeft = argLeft.localPosition;
            origPositionArgRight = argRight.localPosition;
        }
        public override void SetUpMeshOutlineList() {
            meshOutlineList = new List<MeshOutline>() {
                top.gameObject.AddComponent<MeshOutline>(),
                argLeft.gameObject.AddComponent<MeshOutline>(),
                argRight.gameObject.AddComponent<MeshOutline>()
                };
        }

        public override float GetBlockHorizontalSize() {
            return argRight.localScale.x + argLeft.localScale.x + 0.5f; // 0.5f is for top being cut off
        }

        public override float GetBlockVerticalSize() {
            return transform.localScale.y;
        }

        public override Vector3 GetCenterPosition() {
            Debug.Log(argLeft.localPosition + argRight.localPosition);
            return (argRight.localPosition + argLeft.localPosition) / 2.0f;
        }

        public override void ResizeObjectMesh() {
            ResizeArgLeft();
            ResizeArgRight();
        }

        // private methods

        private void ResizeArgLeft() {
            // need to resize arg right based upon horizontal size of arg
            Vector3 rescale = origScaleArg;
            Vector3 reposition = origPositionArgLeft;
            float? horizontalSize = GetMyCodeBlock().GetArgAsCodeBlockAt(0)?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize;
                // 
                reposition.x = reposition.x - (rescale.x - 0.5f) / 2.0f;
            }
            argLeft.localPosition = reposition;
            argLeft.localScale = rescale;
        }

        private void ResizeArgRight() {
            // need to resize arg right based upon horizontal size of arg
            Vector3 rescale = origScaleArg;
            Vector3 reposition = origPositionArgRight;
            float? horizontalSize = GetMyCodeBlock().GetArgAsCodeBlockAt(1)?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize;
                // 
                reposition.x = reposition.x + (rescale.x - 0.5f) / 2.0f;
            }
            argRight.localPosition = reposition;
            argRight.localScale = rescale;
        }
    }
}
