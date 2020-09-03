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
            return argRight.localScale.x + argLeft.localScale.x + top.localScale.x;
        }

        public override float GetBlockVerticalSize() {
            return transform.localScale.y;
        }

        // left bounds minux right 
        public override Vector3 GetCenterPosition() {
            Vector3 leftB = argLeft.localPosition;
            leftB.x -= (argLeft.localScale.x / 2.0f);
            Vector3 rightB = argRight.localPosition;
            rightB.x += (argRight.localScale.x / 2.0f);

            return (rightB + leftB) / 2.0f;
        }

        protected override void ResizeObjectMesh() {
            ResizeArgLeft();
            ResizeArgRight();
        }

        // private methods

        private void ResizeArgLeft() {
            // need to resize arg right based upon horizontal size of arg
            Vector3 rescale = origScaleArg;
            Vector3 reposition = origPositionArgLeft;
            SNAPCOLTYPEDESCRIPTION snalColDescIndex = GetMyCodeBlock().GetType() == typeof(ConditionalCodeBlock) ?
                SNAPCOLTYPEDESCRIPTION.LeftOfConditional :
                SNAPCOLTYPEDESCRIPTION.LeftNumber;

            CodeBlock cb1 = GetMyCodeBlock().GetArgAsCodeBlock(snalColDescIndex);
            if (cb1 != null) {
                cb1.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            }
            float? horizontalSize = GetMyCodeBlock().GetArgAsCodeBlock(snalColDescIndex)?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();

            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize;
                reposition.x = reposition.x - (rescale.x - 0.5f) / 2.0f;
            }
            argLeft.localPosition = reposition;
            argLeft.localScale = rescale;
        }

        private void ResizeArgRight() {
            // need to resize arg right based upon horizontal size of arg
            Vector3 rescale = origScaleArg;
            Vector3 reposition = origPositionArgRight;
            SNAPCOLTYPEDESCRIPTION snalColDescIndex = GetMyCodeBlock().GetType() == typeof(ConditionalCodeBlock) ?
                SNAPCOLTYPEDESCRIPTION.RightOfConditional :
                SNAPCOLTYPEDESCRIPTION.RightNumber;
            float? horizontalSize = GetMyCodeBlock().GetArgAsCodeBlock(snalColDescIndex)?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();

            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize;
                reposition.x = reposition.x - (rescale.x - 0.5f) / 2.0f;
            }
            argRight.localPosition = reposition;
            argRight.localScale = rescale;
        }
    }
}
