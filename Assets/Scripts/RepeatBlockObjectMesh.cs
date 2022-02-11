using UnityEngine;

namespace MoveToCode {
    public class RepeatBlockObjectMesh : SingleControlFlowBlockObjectMesh {
        protected override void ResizeArgRight() {
            Vector3 rescale = origScaleArgRight;
            Vector3 reposition = origPositionArgRight;
            float? horizontalSize = GetComponent<SnapColliderGroup>().SnapColliderSet[CommonSCKeys.RightNumber]?.MyCodeBlockArg?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize / 0.5f;
                reposition.x = reposition.x + ((float)horizontalSize - 0.5f) / 2f; // horizontal is in units of real world
            }
            argRight.localPosition = reposition;
            argRight.localScale = rescale;
        }
    }
}
