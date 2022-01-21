using UnityEngine;
namespace MoveToCode {
    public class TurnInstruction : SnappableStandAloneInstruction {
        CodeBlockEnums.Turn output;
        BabyVirtualKuriController babyVirtualKuriController { get; } = (BabyVirtualKuriController)Object.FindObjectOfType(typeof(BabyVirtualKuriController));
        float turnAngle = 90f;

        public TurnInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
            output = (CodeBlockEnums.Turn)(GetArgument(CommonSCKeys.Turn)?.EvaluateArgument() as TurnDataType).GetValue();
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            if (output == CodeBlockEnums.Turn.Left) {
                babyVirtualKuriController.TurnOverTime(-turnAngle);
            }
            else {
                babyVirtualKuriController.TurnOverTime(turnAngle);
            }
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            return "Turn";
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>" + ToString() + "</color>(", GetArgument(CommonSCKeys.Turn)?.DescriptiveInstructionToString() + ")");
        }
    }
}