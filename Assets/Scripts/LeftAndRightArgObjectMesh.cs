using Microsoft.MixedReality.Toolkit.Utilities;
using System;
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
            return 2;//argRight.localScale.x + argLeft.localScale.x + top.localScale.x;
        }

        public override float GetBlockVerticalSize() {
            return 0.5f;//transform.localScale.y;
        }

        // left bounds minux right 
        public override Vector3 GetCenterPosition() {
            Vector3 leftB = argLeft.localPosition;
            leftB.x -= (argLeft.localScale.x / 2.0f);
            Vector3 rightB = argRight.localPosition;
            rightB.x += (argRight.localScale.x / 2.0f);
            return top.localPosition;
            //return (rightB + leftB) / 2.0f;
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
            KeyValuePair<Type, int> snalColDescIndex = GetMyCodeBlock().GetType() == typeof(ConditionalCodeBlock) ?
                CommonSCKeys.LeftConditional :
                CommonSCKeys.LeftNumber;

            float? horizontalSize = GetComponent<SnapColliderGroup>().SnapColliderSet[snalColDescIndex]?.MyCodeBlockArg?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();

            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize / 0.5f;                
                reposition.x = reposition.x - ((float)horizontalSize - 0.5f) / 2f; // horizontal is in units of real world
            }
            argLeft.localPosition = reposition;
            argLeft.localScale = rescale;
        }

        private void ResizeArgRight() {
            // need to resize arg right based upon horizontal size of arg
            Vector3 rescale = origScaleArg;        // this is all Vector3.one
            Vector3 reposition = origPositionArgRight;  // this is always 0.75, 0, 0

            KeyValuePair<Type, int> snalColDescIndex = GetMyCodeBlock().GetType() == typeof(ConditionalCodeBlock) ?
                CommonSCKeys.RightConditional :
                CommonSCKeys.RightNumber;

            float? horizontalSize = GetComponent<SnapColliderGroup>().SnapColliderSet[snalColDescIndex]?.MyCodeBlockArg?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            
            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize / 0.5f;                
                reposition.x = reposition.x + ((float)horizontalSize - 0.5f) / 2f; // horizontal is in units of real world
            }
            argRight.localPosition = reposition;
            argRight.localScale = rescale;
        }
    }
}
