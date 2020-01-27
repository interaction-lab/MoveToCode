namespace MoveToCode {
    public class ArgumentSnapCollider : SnapCollider {
        public int myInstructionPosition = 0;

        public override void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock) {
            myCodeBlock.SetArgumentBlockAt(collidedCodeBlock, myInstructionPosition, transform.localPosition);
        }

        public override bool IsSnappableToThisSnapColliderType(CodeBlock collidedCodeBlock) {
            return collidedCodeBlock != null && HasCompatibleType(collidedCodeBlock.GetMyInternalIArgument());
        }
    }
}
