using UnityEngine;

namespace MoveToCode {
    public class InstructionSnapCollider : SnapCollider {
        public override void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock) {
            // maybe quadrant round for snapping instructions due to if statements?
            Vector3 relationToParent = transform.localPosition + collidedCodeBlock.GetSnapToParentPosition();
            relationToParent.y = -1f;
            myCodeBlock.SetNextCodeBlock(collidedCodeBlock, relationToParent);
        }
    }
}
