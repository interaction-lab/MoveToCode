using UnityEngine;

namespace MoveToCode {
    public abstract class CodeBlockObjectMesh : MonoBehaviour {
        public abstract void AlertInstructionChanged();
        public abstract Transform GetExitInstructionParentTransform();
        public abstract void SetUpMeshOutline(Material outlineMaterial);
        public abstract void ToggleOutline(bool on);
        public abstract bool IsOutlineSetUp();
    }
}