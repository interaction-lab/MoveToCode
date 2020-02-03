using UnityEngine;

namespace MoveToCode {
    public class ExitInstructionSnapCollider : SnapCollider {
        public override void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock) {
            // need to grab position relative to parent
            Vector3 relationToParent = transform.localPosition + collidedCodeBlock.GetSnapToParentPosition();
            relationToParent.y = -1f;
            (myCodeBlock as ControlFlowCodeBlock).SetExitCodeBlock(collidedCodeBlock, relationToParent);
        }
    }
}