﻿namespace MoveToCode {
    public class WhileInstruction : SingleControlFlowInstruction {

        public WhileInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override InstructionReturnValue RunInstruction() {
            if (!exitInstructionAddedToStack) {
                Interpreter.instance.AddToInstructionStack(GetNextInstruction());
                exitInstructionAddedToStack = true;
            }
            EvaluateArgumentList();
            if (conditionIsTrue) {
                // put me on top of stack for when while loop ends
                Interpreter.instance.AddToInstructionStack(this);
                return new InstructionReturnValue(null, GetNestedInstruction());
            }
            return null; // done with loop
        }

        public override string ToString() {
            return "while ";
        }
    }
}