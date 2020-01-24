using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public abstract class CodeBlockObjectMesh : MonoBehaviour {
        public abstract void AlertInstructionChanged();
        public abstract Transform GetExitInstructionParentTransform();
    }
}