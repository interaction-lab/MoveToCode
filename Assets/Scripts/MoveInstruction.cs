using UnityEngine;
namespace MoveToCode {
    public class MoveInstruction : SnappableStandAloneInstruction {
        CodeBlockEnums.Move output;
        BabyVirtualKuriController babyVirtualKuriController { get; } = (BabyVirtualKuriController)Object.FindObjectOfType(typeof(BabyVirtualKuriController));
        float moveDist = 1f;

        public MoveInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
            output = (CodeBlockEnums.Move)(GetArgument(CommonSCKeys.Move)?.EvaluateArgument() as MoveDataType).GetValue();
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            if (output == CodeBlockEnums.Move.Forward) {
                babyVirtualKuriController.MoveOverTime(moveDist);
            }
            else {
                babyVirtualKuriController.MoveOverTime(-moveDist);
            }
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            return "Move";
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>" + ToString() + "</color>(", GetArgument(CommonSCKeys.Move)?.DescriptiveInstructionToString() + ")");
        }
    }
}