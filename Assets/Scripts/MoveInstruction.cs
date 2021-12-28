using UnityEngine;
namespace MoveToCode {
    public class MoveInstruction : SnappableStandAloneInstruction {
        string output;

        public MoveInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
            //output = GetArgument(CommonSCKeys.Printable)?.EvaluateArgument()?.ToString();
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            BabyKuriManager.instance.kuriController.TakeMovementAction();
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