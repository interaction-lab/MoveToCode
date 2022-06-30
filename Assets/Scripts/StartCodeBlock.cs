using UnityEngine;
namespace MoveToCode {
    public class StartCodeBlock : InstructionCodeBlock {
        public static StartCodeBlock m_Instance;
        private static bool m_ShuttingDown = false;
        private static object m_Lock = new object();
        public Vector3 startPos;

        public static StartCodeBlock instance {
            get {
                if (m_ShuttingDown) {
                    return null;
                }
                lock (m_Lock) {
                    if (m_Instance == null) {
                        m_Instance = (StartCodeBlock)FindObjectOfType(typeof(StartCodeBlock));
                    }
                    return m_Instance;
                }
            }
        }

        public Vector3 GetStartPos() {
            if (startPos == Vector3.zero) {
                startPos = transform.position;
            }
            return startPos;
        }

        public override string ToString() {
            return StartInstruction.startString;
        }

        public void ResetToLocalStartLocation(){
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new StartInstruction(this);
        }
    }
}