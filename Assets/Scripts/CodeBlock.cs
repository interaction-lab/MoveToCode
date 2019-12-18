using UnityEngine;

namespace MoveToCode {
    public class CodeBlock : MonoBehaviour {
        public Instruction myInstruction;
        CodeBlock childCodeBlock;

        public void SetNextInstruction(Instruction iIn) {
            myInstruction.SetNextInstruction(iIn);
        }

        public void RemoveChildBlockAndAttachNew(CodeBlock newChildBlock) {
            RemoveChildBlock();
            AddNewChildBlock(newChildBlock);
        }

        private void AddNewChildBlock(CodeBlock newChildBlock) {
            childCodeBlock = newChildBlock;
            if (newChildBlock) {
                newChildBlock.transform.SetParent(transform);
                newChildBlock.transform.localPosition = Vector3.down;
            }
        }

        public void RemoveChildBlock() {
            childCodeBlock?.transform.SetParent(CodeBlockManager.instance.transform);
        }
    }
}