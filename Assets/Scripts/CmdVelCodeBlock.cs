using RosSharp.RosBridgeClient;

namespace MoveToCode {
    public class CmdVelCodeBlock : StandAloneInstructionCodeBlock {
        protected override void SetMyBlockInternalArg() {
            myBlockInternalArg = new CmdVelInstruction(this);
        }

        public CmdVelPublisher GetCmdVelPublisher() {
            return FindObjectOfType<RosConnector>().GetComponent<CmdVelPublisher>();
        }
    }
}