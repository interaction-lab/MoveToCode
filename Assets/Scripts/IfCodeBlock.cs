using UnityEngine;

namespace MoveToCode {
    public class IfCodeBlock : ControlFlowCodeBlock {
        public override int GetBlockVerticalSize() {
            return 3;
        }

        public override Vector3 GetSnapToParentPosition() {
            return new Vector3(0.25f, 0, 0);
        }

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new IfInstruction();
        }
    }
}