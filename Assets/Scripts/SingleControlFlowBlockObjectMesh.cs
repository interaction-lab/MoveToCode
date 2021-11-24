using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SingleControlFlowBlockObjectMesh : ControlFlowBlockObjectMesh {
        Transform top, argRight, side, bot;
        Vector3 origScaleArgRight;
        Vector3 origPositionArgRight;

        float TopSizeHBC { get; } = 0.8f;
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
            return GetSizeOfInsideInstructionChain() + GetSizeOfExitInstructionChain() + 0.5f /*top*/ + 0.5f /*bot*/;
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

            // TODO: this is done slightly differently, this should really be a method
            CodeBlockObjectMesh obMesh = GetComponent<SnapColliderGroup>().SnapColliderSet[CommonSCKeys.Conditional]?.MyCodeBlockArg?.GetCodeBlockObjectMesh();
            if (obMesh != null) {
                rescale.x = obMesh.GetBlockHorizontalSize();
                reposition.x = reposition.x + (rescale.x - 0.5f) / 2.0f;
            }
            argRight.localPosition = reposition;
            argRight.localScale = rescale;
        }
    }
}
