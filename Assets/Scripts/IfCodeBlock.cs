using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class IfCodeBlock : ControlFlowCodeBlock {

        // need my CodeBlockObjectMesh
        //  - expand on add
        //  - shrink on remove
        //

        // next instruction is on true
        // have holder for potentialNextInstruction; treat this as an argument and add on evaluate
        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new IfInstruction();
        }
    }
}