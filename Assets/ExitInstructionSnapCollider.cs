using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MoveToCode {
    public class ExitInstructionSnapCollider : InstructionSnapCollider {
        public override void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock) {
            // need to grab position relative to parent
            Vector3 relationToParent = transform.localPosition;
            relationToParent.y = -1f;
            ControlFlowCodeBlock castedMyBlock = myCodeBlock as ControlFlowCodeBlock;
            Assert.IsNotNull(castedMyBlock);
            castedMyBlock.SetExitCodeBlock(collidedCodeBlock, Vector3.down);
        }
    }
}