using UnityEngine;

namespace MoveToCode {
    public class IfCodeBlockObjectMesh : CodeBlockObjectMesh {
        Transform top, side, bottom;
        float scaleValue = 1.0f;
        float translateValue = 0.5f;

        private void Awake() {
            top = transform.GetChild(0);
            side = transform.GetChild(1);
            bottom = transform.GetChild(2);
        }

        public override void AlertInstructionAdded() {
            ResizeMeshes(scaleValue, translateValue);
        }

        public override void AlertInstructionRemoved() {
            ResizeMeshes(-scaleValue, -translateValue);
        }

        public override Transform GetExitInstructionParentTransform() {
            return bottom;
        }

        private void ResizeMeshes(float scaleVal, float transVal) {
            Vector3 scaler = side.localScale;
            scaler.y += scaleVal;
            side.localScale = scaler;
            Vector3 translate = side.localPosition;
            translate.y -= transVal;
            side.localPosition = translate;

            // need to move down bottom also
            translate = bottom.localPosition;
            translate.y -= scaleVal;
            bottom.localPosition = translate;
        }
    }
}
