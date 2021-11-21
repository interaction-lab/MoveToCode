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
            return top.localScale.x + argRight.localScale.x / 2f; // TODO: fix when rescaling/repositioning
        }

        public override float GetBlockVerticalSize() {
            return 0.5f + FindChainSize(GetMyCodeBlock().GetArgumentFromDict(CommonSCKeys.Next));
        }

        public override Vector3 GetCenterPosition() {
            return Vector3.zero; // todo later
        }

        protected override void ResizeObjectMesh() {
            // need to resize arg right based upon horizontal size of arg
            Vector3 rescale = origScaleArgRight;        // this is all Vector3.one
            Vector3 reposition = origPositionArgRight;  // this is always 0.75, 0, 0

            float? horizontalSize = GetComponent<SnapColliderGroup>().SnapColliderSet[CommonSCKeys.Printable]?.MyCodeBlockArg?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            
            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize / 0.5f;                
                reposition.x = reposition.x + ((float)horizontalSize - 0.5f) / 2f; // horizontal is in units of real world
            }
            argRight.localPosition = reposition;
            argRight.localScale = rescale;
        }
    }
}
