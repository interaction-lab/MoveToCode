using UnityEngine;
namespace MoveToCode {
    public class SetColorInstruction : SnappableStandAloneInstruction {

        Color output;
        BabyVirtualKuriController babyVirtualKuriController { get; } = (BabyVirtualKuriController)Object.FindObjectOfType(typeof(BabyVirtualKuriController));

        public SetColorInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
            output = (Color)(GetArgument(CommonSCKeys.Color)?.EvaluateArgument() as ColorDataType).GetValue();
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            BabyKuriManager.instance.ChangeKuriColor(output);
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            return "Set Color";
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>" + ToString() + "</color>(", GetArgument(CommonSCKeys.Color)?.DescriptiveInstructionToString() + ")");
        }
    }
}