using UnityEngine;
namespace MoveToCode {
    public class MoveInstruction : SnappableStandAloneInstruction {
        CodeBlockEnums.Move output;

        public MoveInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void EvaluateArgumentsOfInstruction() {
            output = (CodeBlockEnums.Move)(GetArgument(CommonSCKeys.Move)?.EvaluateArgument() as MoveDataType).GetValue();
        }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            Debug.Log(output);
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