using UnityEngine;
using RosSharp.RosBridgeClient;
namespace MoveToCode {
    public class SetColorInstruction : SnappableStandAloneInstruction {

        Color output;
        ChestLedPublisher cledp = null;
        ChestLedPublisher Cledp {
            get {
                if (cledp == null) {
                    cledp = GameObject.FindObjectOfType<ChestLedPublisher>();
                }
                return cledp;
            }
        }

        BabyVirtualKuriController babyVirtualKuriController { get; } = (BabyVirtualKuriController)Object.FindObjectOfType(typeof(BabyVirtualKuriController));

        public SetColorInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
            output = (Color)(GetArgument(CommonSCKeys.Color)?.EvaluateArgument() as ColorDataType).GetValue();
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            if (OptionSelectionManager.instance.usePhysicalKuri) {
                Cledp.SetColor(output);
            }
            else {
                BabyKuriManager.instance.ChangeKuriColor(output);
            }
            return new InstructionReturnValue(null, GetNextInstruction());
        }

        public override string ToString() {
            return "Set Color";
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>" + ToString() + "</color>(", GetArgument(CommonSCKeys.Color)?.DescriptiveInstructionToString() + ")");
        }

        public override string ToJSON() {
            return string.Join(",", new string[] {
                "{\"name\": \"" + ToString() + "\"",
                "\"type\": \"" + GetType().ToString() + "\"",
                "\"args\":{\"color\": " + GetArgumentJSON(CommonSCKeys.Color),
                "\"next\": " + GetArgumentJSON(CommonSCKeys.Next) + "}}"
            });
        }
    }
}