using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ArgInstructionSnapCollider : ArgumentSnapCollider {
        public override void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock) {
            Vector3 relationToParent = transform.localPosition + collidedCodeBlock.GetSnapToParentPosition();
            relationToParent.y = -1f;
            myCodeBlock.SetArgumentBlockAt(collidedCodeBlock, myArgumentPosition, transform.localPosition);
            collidedCodeBlock.transform.SnapToParent(transform.parent, relationToParent);
        }

    }
}