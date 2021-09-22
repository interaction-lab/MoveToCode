using RosSharp.RosBridgeClient;
using UnityEngine;

namespace MoveToCode {
    public class CmdVelInstruction : SnappableStandAloneInstruction {

        CmdVelPublisher pub;
        DirectionDataType.DIRECTION direction = DirectionDataType.DIRECTION.NULL;
        public CmdVelInstruction(CodeBlock cbIn) : base(cbIn) {
            // TODO: really ugly way to get this in here
            CmdVelCodeBlock tmp = (CmdVelCodeBlock)cbIn;
            pub = tmp.GetCmdVelPublisher();
        }

        public override void EvaluateArgumentsOfInstruction() {
            direction = (DirectionDataType.DIRECTION)GetArgument(CommonSCKeys.Direction)?.EvaluateArgument().GetValue();
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            Vector2 _ = ConvertDirectionDict(direction);
            pub.PublishCmdVel(_.x, _.y);
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            return "Move " + (direction == DirectionDataType.DIRECTION.NULL ? "" : direction.ToString());
        }


        private Vector2 ConvertDirectionDict(DirectionDataType.DIRECTION dIn) {
            Vector2 output = Vector2.zero;
            switch (dIn) {
                case DirectionDataType.DIRECTION.Up:
                    output.x = 1;
                    break;
                case DirectionDataType.DIRECTION.Down:
                    output.x = -1;
                    break;
                case DirectionDataType.DIRECTION.Left:
                    output.y = -90;
                    break;
                case DirectionDataType.DIRECTION.Right:
                    output.y = 90;
                    break;
            }
            return output;

        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>" + ToString() + "</color>(", GetArgument(CommonSCKeys.Printable)?.DescriptiveInstructionToString() + ")");
        }
    }
}