using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class CodeBlock : MonoBehaviour {
        public Instruction myInstruction;
        CodeBlock parentCodeBlock;

        public void SetNextInstruction(Instruction iIn) {
            myInstruction.SetNextInstruction(iIn);
        }

        public void RemoveCurParentAndAttachNewParentBlock(CodeBlock parentCodeBlockIn) {
            RemoveFromParentCodeBlock();
            SetParentBlocksNextInstruction(null);
            SetThisCodeBlocksParentAndPosition(parentCodeBlockIn);
            SetParentBlocksNextInstruction(myInstruction);
        }

        private void SetThisCodeBlocksParentAndPosition(CodeBlock parentCodeBlockIn) {
            transform.SetParent(parentCodeBlock ?
                parentCodeBlock.transform :
                CodeBlockManager.instance.transform);
            transform.localPosition = Vector3.down;
            parentCodeBlock = parentCodeBlockIn;
        }

        public void RemoveFromParentCodeBlock() {
            parentCodeBlock?.SetNextInstruction(null);
        }

        public void SetParentBlocksNextInstruction(Instruction iIn) {
            parentCodeBlock?.SetNextInstruction(myInstruction);
        }
    }
}