using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public abstract class CodeBlockObjectMesh : MonoBehaviour {
        public abstract void AlertInstructionAdded();
        public abstract void AlertInstructionRemoved();
        public abstract Transform GetExitInstructionParentTransform();
    }
}