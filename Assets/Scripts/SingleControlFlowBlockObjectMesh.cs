using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SingleControlFlowBlockObjectMesh : ControlFlowBlockObjectMesh {
        protected Transform top, argRight, side, bot;
        protected Vector3 origScaleArgRight;
        protected Vector3 origPositionArgRight;

        float TopSizeHBC { get; } = 0.8f;
        float TopSizeVBC { get; } = 0.5f;
        float BotSizeVBC { get; } = 0.5f;
        float SideSizeVBC { get; } = 1.5f;
        float ArgRightHBC {
            get {
                return argRight.localScale.x * 0.5f;
            }
        }
        float SideSizeHBC {
            get {
                return side.localScale.x;
            }
        }

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
            return GetSizeOfInsideInstructionChain() + GetSizeOfExitInstructionChain() + TopSizeVBC + BotSizeVBC;
        }

        public override float GetBlockHorizontalSize() {
            return SideSizeHBC + TopSizeHBC + ArgRightHBC;
        }

        public override Vector3 GetCenterPosition() {
            return Vector3.zero; // TODO: figure out what calls this even and why
        }

        protected override void ResizeObjectMesh() {
            ResizeArgRight();
            ResizeSide();
        }

        // private helpers
        private float GetSizeOfInsideInstructionChain() {
            return FindChainSize(GetMyCodeBlock().GetArgumentFromDict(CommonSCKeys.Nested));
        }
        private float GetSizeOfExitInstructionChain() {
            return FindChainSize(GetMyCodeBlock().GetArgumentFromDict(CommonSCKeys.Next)) + 0.5f;
        }

        private void ResizeSide() {
            float internalSize = GetSizeOfInsideInstructionChain();

            Vector3 scaler = side.localScale;
            scaler.y = internalSize + SideSizeVBC;
            side.localScale = scaler;

            scaler = bot.localPosition;
            scaler.y = -(internalSize + 1f);
            bot.localPosition = scaler;

            scaler = side.localPosition;
            scaler.y = (top.localPosition.y + bot.localPosition.y) / 2.0f;
            side.localPosition = scaler;
        }
        protected virtual void ResizeArgRight() {
            Vector3 rescale = origScaleArgRight;
            Vector3 reposition = origPositionArgRight;
            float? horizontalSize = GetComponent<SnapColliderGroup>().SnapColliderSet[CommonSCKeys.Conditional]?.MyCodeBlockArg?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize / 0.5f;
                reposition.x = reposition.x + ((float)horizontalSize - 0.5f) / 2f; // horizontal is in units of real world
            }
            argRight.localPosition = reposition;
            argRight.localScale = rescale;
        }
    }
}
