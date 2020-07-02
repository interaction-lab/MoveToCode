using System;
using System.Collections.Generic;

namespace MoveToCode
{
    public abstract class ControlFlowInstruction : StandAloneInstruction
    {
        protected bool conditionIsTrue;
        protected bool exitInstructionAddedToStack;

        protected abstract StandAloneInstruction GetNestedInstruction(); //exit -> nested
        protected abstract string GetNestedInstructionsAsString();

        public ControlFlowInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void ResestInternalState()
        {
            exitInstructionAddedToStack = false;
        }

        public override void EvaluateArgumentList()
        {
            IDataType d = (GetArgumentAt(1) as ConditionalInstruction)?.RunInstruction().GetReturnDataVal();
            if (d != null)
            {
                conditionIsTrue = (bool)d.GetValue();
            }
        }

        public override void SetUpArgPosToCompatability()
        {
            argPosToCompatability = new List<List<Type>> {
                new List<Type>{
                    typeof(StandAloneInstruction)
                },
                new List<Type> {
                    typeof(ConditionalInstruction)
                },
                new List<Type> {
                    typeof(StandAloneInstruction)
                }
            };
        }

        public override void SetUpArgDescriptionList()
        {
            argDescriptionList = new List<string> { "Nested Instruction", "Conditional Instruction", "Next Instruction" }; //next->nested, exit->next
        }
    }
}
