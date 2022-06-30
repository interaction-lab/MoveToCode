using RosSharp.RosBridgeClient;
using System.Collections.Generic;
using UnityEngine;
namespace MoveToCode {
    public class MoveInstruction : SnappableStandAloneInstruction {
        CodeBlockEnums.Move output;
        BabyVirtualKuriController babyVirtualKuriController { get; } = (BabyVirtualKuriController)Object.FindObjectOfType(typeof(BabyVirtualKuriController));
        float moveDist = 0.34f;

        MoveTwistPublisher mtp = null;
        MoveTwistPublisher Mtp {
            get {
                if (mtp == null) {
                    mtp = GameObject.FindObjectOfType<MoveTwistPublisher>();
                }
                return mtp;
            }
        }
        MazeManager _mazeManager;
        MazeManager MazeManagerInstance {
            get {
                if (_mazeManager == null) {
                    _mazeManager = MazeManager.instance;
                }
                return _mazeManager;
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
                Mtp.UpdateMessage(moveDist, 0);
            }
            else {
                Mtp.UpdateMessage(-moveDist, 0);
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

        public override string ToJSON() {
            return string.Join(",", new string[] {
                "{\"name\": \"" + ToString() + "\"",
                "\"type\": \"" + GetType().ToString() + "\"",
                "\"args\":{\"next\": " + GetArgumentJSON(CommonSCKeys.Next),
                "\"move\": " + GetArgumentJSON(CommonSCKeys.Move) + "}}"
            });
        }
    }
}