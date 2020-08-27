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
            return GetSizeOfInsideInstructionChain() + GetSizeOfExitInstructionChain() + top.localScale.y + bot.localScale.y;
        }

        public override float GetBlockHorizontalSize() {
            return side.localScale.x + top.localScale.x + argRight.localScale.x;
        }

        public override Vector3 GetCenterPosition() {
            return Vector3.zero; // maybe this is right?
        }

        protected override void ResizeObjectMesh() {
            ResizeArgRight();
            ResizeSide();
        }

        // private helpers
        private float GetSizeOfInsideInstructionChain() {
            return FindChainSize(GetMyCodeBlock().GetArgAsIArg(IARG.Nested));
        }
        private float GetSizeOfExitInstructionChain() {
            return FindChainSize(GetMyCodeBlock().GetArgAsIArg(IARG.Next)) + 0.5f;
        }

        private void ResizeSide() {
            float internalSize = GetSizeOfInsideInstructionChain();

            Vector3 scaler = side.localScale;
            scaler.y = internalSize + 1.5f;
            side.localScale = scaler;

            scaler = bot.localPosition;
            scaler.y = -(internalSize + 1f);
            bot.localPosition = scaler;

            scaler = side.localPosition;
            scaler.y = (top.localPosition.y + bot.localPosition.y) / 2.0f;
            side.localPosition = scaler;
        }
        private void ResizeArgRight() {
            Vector3 rescale = origScaleArgRight;
            Vector3 reposition = origPositionArgRight;
            CodeBlockObjectMesh obMesh = GetMyCodeBlock().GetArgAsCodeBlock(IARG.Conditional)?.GetCodeBlockObjectMesh();
            if (obMesh != null) {
                rescale.x = obMesh.GetBlockHorizontalSize();
                reposition.x = reposition.x + (rescale.x - 0.5f) / 2.0f;
            }
            argRight.localPosition = reposition;
            argRight.localScale = rescale;
        }
    }
}
