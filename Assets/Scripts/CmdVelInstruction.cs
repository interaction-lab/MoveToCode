using RosSharp.RosBridgeClient;
using UnityEngine;

namespace MoveToCode {
    public class CmdVelInstruction : SnappableStandAloneInstruction {

        CmdVelPublisher pub;
        public CmdVelInstruction(CodeBlock cbIn) : base(cbIn) {
            // TODO: really ugly way to get this in here
            CmdVelCodeBlock tmp = (CmdVelCodeBlock)cbIn;
            pub = tmp.GetCmdVelPublisher();
        }

        public override void EvaluateArgumentsOfInstruction() {

        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            pub.PublishCmdVel(1, 0);
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            return "Move";
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>" + ToString() + "</color>(", GetArgument(CommonSCKeys.Printable)?.DescriptiveInstructionToString() + ")");
        }
    }
}