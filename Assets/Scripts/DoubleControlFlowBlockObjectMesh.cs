using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class DoubleControlFlowBlockObjectMesh : ControlFlowBlockObjectMesh {
        Transform top, sideTop, mid, sideBot, bot;

        public override void SetUpObject() {
            top = transform.GetChild(0);
            sideTop = transform.GetChild(1);
            mid = transform.GetChild(2);
            sideBot = transform.GetChild(3);
            bot = transform.GetChild(4);
        }

        public override void SetUpMeshOutlineList() {
            meshOutlineList = new List<MeshOutline>() {
                 top.gameObject.AddComponent<MeshOutline>(),
                 sideTop.gameObject.AddComponent<MeshOutline>(),
                 mid.gameObject.AddComponent<MeshOutline>(),
                 sideBot.gameObject.AddComponent<MeshOutline>(),
                 bot.gameObject.AddComponent<MeshOutline>()
                 };
        }

        public override float GetBlockVerticalSize() {
            return FindTopVerticalSize() + FindBotVerticalSize() + 2;
        }

        public override float GetBlockHorizontalSize() {
            return 3; // todo will do later
        }

        protected override void ResizeObjectMesh() {
            ResizeTop();
            ResizeBot();
        }

        // private methods

        private float FindTopVerticalSize() {
            return FindChainSize(GetMyCodeBlock().GetArgumentFromDict(string.Nested));
        }

        private float FindBotVerticalSize() {
            return FindChainSize(GetMyCodeBlock().GetArgumentFromDict(string.Nested));
        }

        public override Vector3 GetCenterPosition() {
            return Vector3.zero;// this needs to be remade
        }

        private void ResizeTop() {
            float topVertSize = FindTopVerticalSize();

            Vector3 scaler = sideTop.localScale;
            scaler.y = topVertSize;
            sideTop.localScale = scaler;

            scaler = sideTop.localPosition;
            scaler.y = -(sideTop.localScale.y - 1) / 2;
            sideTop.localPosition = scaler;

            // need to move down bottom also
            scaler = mid.localPosition;
            scaler.y = -topVertSize;
            mid.localPosition = scaler;
        }

        private void ResizeBot() {
            float botVertSize = FindBotVerticalSize();

            Vector3 scaler = sideBot.localScale;
            scaler.y = botVertSize;
            sideBot.localScale = scaler;

            scaler = sideBot.localPosition;
            scaler.y = -(sideBot.localScale.y - 1) / 2; // here 
            sideBot.localPosition = scaler;

            // need to move down bottom also
            scaler = bot.localPosition;
            scaler.y = -botVertSize;
            bot.localPosition = scaler;
        }


    }
}
