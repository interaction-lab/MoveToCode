namespace MoveToCode {
    public class ArgumentSnapCollider : SnapCollider {
        public int myInstructionPosition = 0;

        public override void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock) {
            myCodeBlock.SetArgumentBlockAt(collidedCodeBlock, myInstructionPosition, transform.localPosition);
        }
    }
}
