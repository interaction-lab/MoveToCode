using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class ArgumentSnapCollider : SnapCollider {
        public int myArgumentPosition = 0;

        public override void DoSnapAction(CodeBlock myCodeBlock, CodeBlock collidedCodeBlock) {
            myCodeBlock.SetArgumentBlockAt(collidedCodeBlock, myArgumentPosition, transform.localPosition);
        }

        protected override List<Type> GetMyCompatibleArgTypes() {
            // grab instruction and get arg compatibility
            // how should I store the compatibility of the instructions in
            if (myCompatibleArgTypes == null) {
                myCompatibleArgTypes = GetMyCodeBlock().GetMyInstruction().GetArgCompatibilityAtPos(myArgumentPosition);
            }
            return myCompatibleArgTypes;
        }
    }
}
