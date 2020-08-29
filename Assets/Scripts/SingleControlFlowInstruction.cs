using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class SingleControlFlowInstruction : ControlFlowInstruction {
        public SingleControlFlowInstruction(CodeBlock cbIn) : base(cbIn) { }

        public override void SetUpArgToSnapColliderDict() {
            argToSnapColliderDict = new Dictionary<SNAPCOLTYPEDESCRIPTION, HashSet<Type>> {
                { SNAPCOLTYPEDESCRIPTION.Nested, new HashSet<Type> { typeof(StandAloneInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.Conditional, new HashSet<Type> {  typeof(ConditionalInstruction) }  },
                { SNAPCOLTYPEDESCRIPTION.Next, new HashSet<Type> { typeof(StandAloneInstruction) }  }
            };
        }

        public override string DescriptiveInstructionToString() {
            return string.Join("", "<color=purple>", ToString(), "</color>", GetArgument(SNAPCOLTYPEDESCRIPTION.Conditional)?.DescriptiveInstructionToString(), ": ", GetNestedInstructionsAsString());
        }
    }
}
