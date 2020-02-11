using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public abstract class ControlFlowBlockObjectMesh : CodeBlockObjectMesh {
        protected float FindChainSize(IArgument aIn) {
            StandAloneInstruction sai = aIn as StandAloneInstruction;
            if (sai != null) {
                return sai.GetCodeBlock().GetCodeBlockObjectMesh().GetBlockVerticalSize(); // todo maybe make this cleaner?
            }
            return 0;
        }
    }
}
