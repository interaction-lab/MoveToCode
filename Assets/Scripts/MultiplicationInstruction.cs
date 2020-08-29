﻿namespace MoveToCode {
    public class MultiplicationInstruction : MathInstruction {
        public MultiplicationInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override InstructionReturnValue RunInstruction() {
            EvaluateArgumentsOfInstruction();
            return new InstructionReturnValue(new FloatDataType(leftNum * rightNum), null);
        }

        public override string GetMathSymbol() {
            return "*";
        }
    }
}