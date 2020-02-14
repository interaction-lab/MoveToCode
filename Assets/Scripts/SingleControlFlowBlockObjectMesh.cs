using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SingleControlFlowBlockObjectMesh : ControlFlowBlockObjectMesh {
        Transform top, argRight, side, bot;
        Vector3 origScaleArgRight;
        Vector3 origPositionArgRight;

        public override void SetUpObject() {
            top = transform.GetChild(0);
            argRight = transform.GetChild(1);
            side = transform.GetChild(2);
            bot = transform.GetChild(3);

            origScaleArgRight = argRight.localScale;
            origPositionArgRight = argRight.localPosition;
        }

        public override void SetUpMeshOutlineList() {
            meshOutlineList = new List<MeshOutline>() {
                top.gameObject.AddComponent<MeshOutline>(),
                argRight.gameObject.AddComponent<MeshOutline>(),
                side.gameObject.AddComponent<MeshOutline>(),
                bot.gameObject.AddComponent<MeshOutline>()
             };
        }

        public override float GetBlockVerticalSize() {
            return FindChainSize(GetMyCodeBlock().GetArgAsIArgumentAt(0)) + 2;
        }

        public override float GetBlockHorizontalSize() {
            return side.localScale.x + top.localScale.x + argRight.localScale.x;
        }

        public override Vector3 GetCenterPosition() {
            return Vector3.zero; // maybe this is right?
        }

        public override void ResizeObjectMesh() {
            ResizeArgRight();
            float verticalSize = GetBlockVerticalSize();

            Vector3 scaler = side.localScale;
            scaler.y = verticalSize;
            side.localScale = scaler;

            scaler = side.localPosition;
            scaler.y = -(side.localScale.y - 1) / 2;
            side.localPosition = scaler;

            // need to move down bottom also
            scaler = bot.localPosition;
            scaler.y = -verticalSize;
            bot.localPosition = scaler;
        }

        // private helpers
        private void ResizeArgRight() {
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
