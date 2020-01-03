using UnityEngine;

namespace MoveToCode {
    public class InstructionSnapCollider : SnapCollider {
        public override void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock) {
            myCodeBlock.SetNextCodeBlock(collidedCodeBlock, Vector3.down);
        }

        public override bool IsSnappableToThisSnapColliderType(CodeBlock collidedCodeBlock) {
            return collidedCodeBlock != null && collidedCodeBlock.IsInstructionCodeBlock(); // TODO: need instructions with no return maybe...
        }
    }
}
