using System;
using UnityEngine;

namespace MoveToCode {
    public class ColorCodeBlock : DataCodeBlock {
        public Color output;
        private MeshRenderer topRend;
        private MeshRenderer TopRend {
            get {
                if (topRend == null) {
                    topRend = transform.GetChild(0).GetComponentInChildren<MeshRenderer>();
                }
                return topRend;
            }
        }

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new ColorDataType(this, output);
            UpdateColor();
        }

        public override void SetOutput(object value) {
            output = (Color)value;
            UpdateColor();
            base.SetOutput(value);
        }

        private void UpdateColor() {
            TopRend.material.color = output;
        }
    }
}