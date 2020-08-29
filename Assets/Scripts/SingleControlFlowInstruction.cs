using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class SingleControlFlowInstruction : ControlFlowInstruction {
        public SingleControlFlowInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void SetUpArgCompatabilityDict() {
            argCompatabilityDict = new Dictionary<IARG, HashSet<Type>> {
                { IARG.Nested, new HashSet<Type> { typeof(StandAloneInstruction) }  },
                { IARG.Conditional, new HashSet<Type> {  typeof(ConditionalInstruction) }  },
                { IARG.Next, new HashSet<Type> { typeof(StandAloneInstruction) }  }
            };
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>", ToString(), "</color>", GetArgument(IARG.Conditional)?.DescriptiveInstructionToString(), ": ", GetNestedInstructionsAsString());
        }
    }
}
