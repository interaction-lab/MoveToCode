using UnityEngine;

namespace MoveToCode {
    public abstract class DataCodeBlock : CodeBlock {
        public override string ToString() {
            return myData.ToString();
        }
        public override bool IsDataCodeBlock() {
            return true;
        }
        public override bool IsInstructionCodeBlock() {
            return false;
        }
    }
}