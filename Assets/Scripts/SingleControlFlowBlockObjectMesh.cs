using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SingleControlFlowBlockObjectMesh : ControlFlowBlockObjectMesh {
        Transform top, side, bot;

        public override void SetUpObject() {
            top = transform.GetChild(0);
            side = transform.GetChild(1);
            bot = transform.GetChild(2);
        }

        public override void SetUpMeshOutlineList() {
            meshOutlineList = new List<MeshOutline>() { top.gameObject.AddComponent<MeshOutline>() };
        }

        public override float GetBlockVerticalSize() {
            return FindChainSize(GetMyCodeBlock().GetArgAsIArgumentAt(0)) + 2;
        }

        public override float GetBlockHorizontalSize() {
            return 3f;  // todo fix this later
        }

        public override void ResizeObjectMesh() {
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
    }
}
