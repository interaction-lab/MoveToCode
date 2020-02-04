using UnityEngine;
namespace MoveToCode {
    public abstract class SingleControlFlowCodeBlock : ControlFlowCodeBlock {
        public override int GetBlockVerticalSize() {
            return 3;
        }

        public override Vector3 GetSnapToParentPosition() {
            return new Vector3(0.25f, 0, 0);
        }

    }
}