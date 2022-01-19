using UnityEngine;

namespace MoveToCode {
    public class PrintObjectMesh : SingleRightArgObjectMesh {
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
