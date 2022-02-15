using RosSharp.RosBridgeClient;
using UnityEngine;
namespace MoveToCode {
    public class MoveInstruction : SnappableStandAloneInstruction {
        CodeBlockEnums.Move output;
        BabyVirtualKuriController babyVirtualKuriController { get; } = (BabyVirtualKuriController)Object.FindObjectOfType(typeof(BabyVirtualKuriController));
        float moveDist = 1f;

        MoveTwistPublisher mtp = null;
        MoveTwistPublisher Mtp {
            get {
                if(mtp == null) {
                    mtp = GameObject.FindObjectOfType<MoveTwistPublisher>();
                }
                return mtp;
            }
        }

        public MoveInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
            output = (CodeBlockEnums.Move)(GetArgument(CommonSCKeys.Move)?.EvaluateArgument() as MoveDataType).GetValue();
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            if (OptionSelectionManager.instance.usePhysicalKuri) {
                MovePhysicalKuri();
            }
            else {
                MoveVirtualBabyKuri();
            }
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        private void MovePhysicalKuri() {
            if (output == CodeBlockEnums.Move.Forward) {
                Mtp.UpdateMessage(moveDist,0);
            }
            else {
                Mtp.UpdateMessage(-moveDist,0);
            }
        }


        private void MoveVirtualBabyKuri() {
            if (output == CodeBlockEnums.Move.Forward) {
                babyVirtualKuriController.MoveOverTime(moveDist);
            }
            else {
                babyVirtualKuriController.MoveOverTime(-moveDist);
            }
        }

        public override string ToString() {
            return "Move";
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>" + ToString() + "</color>(", GetArgument(CommonSCKeys.Move)?.DescriptiveInstructionToString() + ")");
        }
    }
}