namespace MoveToCode {
    public class StartCodeBlock : InstructionCodeBlock {
        public static StartCodeBlock m_Instance;
        private static bool m_ShuttingDown = false;
        private static object m_Lock = new object();

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

        public override string ToString() {
            return StartInstruction.startString;
        }

        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new StartInstruction(this);
        }
    }
}