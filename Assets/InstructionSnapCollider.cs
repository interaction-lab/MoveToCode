namespace MoveToCode {
    public class InstructionSnapCollider : SnapCollider {
        public override void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock) {
            myCodeBlock.SetNextCodeBlock(collidedCodeBlock);
        }
    }
}
