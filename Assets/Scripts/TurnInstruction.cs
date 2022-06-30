using RosSharp.RosBridgeClient;
using UnityEngine;
namespace MoveToCode {
    public class TurnInstruction : SnappableStandAloneInstruction {
        CodeBlockEnums.Turn output;
        BabyVirtualKuriController babyVirtualKuriController { get; } = (BabyVirtualKuriController)Object.FindObjectOfType(typeof(BabyVirtualKuriController));
        float turnAngle = 90f;


        MoveTwistPublisher mtp = null;
        MoveTwistPublisher Mtp {
            get {
                if (mtp == null) {
                    mtp = GameObject.FindObjectOfType<MoveTwistPublisher>();
                }
                return mtp;
            }
        }

        public TurnInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
            output = (CodeBlockEnums.Turn)(GetArgument(CommonSCKeys.Turn)?.EvaluateArgument() as TurnDataType).GetValue();
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            if (OptionSelectionManager.instance.usePhysicalKuri) {
                TurnPhysicalKuri();
            }
            else {
                TurnVirtualBabyKuri();
            }
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        private void TurnPhysicalKuri() {
            if (output == CodeBlockEnums.Turn.Left) {
                Mtp.UpdateMessage(0, -turnAngle);
            }
            else {
                Mtp.UpdateMessage(0, -turnAngle);
            }
        }

        private void TurnVirtualBabyKuri() {
            if (output == CodeBlockEnums.Turn.Left) {
                babyVirtualKuriController.TurnOverTime(-turnAngle);
            }
            else {
                babyVirtualKuriController.TurnOverTime(turnAngle);
            }
        }

        public override string ToString() {
            return "Turn";
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>" + ToString() + "</color>(", GetArgument(CommonSCKeys.Turn)?.DescriptiveInstructionToString() + ")");
        }

        public override string ToJSON() {
            return string.Join(",", new string[] {
                "{\"name\": \"" + ToString() + "\"",
                "\"type\": \"" + GetType().ToString() + "\"",
                "\"args\":{\"turn\": " + GetArgumentJSON(CommonSCKeys.Turn),
                "\"next\": " + GetArgumentJSON(CommonSCKeys.Next) + "}}"
            });
        }
    }
}