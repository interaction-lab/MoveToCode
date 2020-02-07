using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class SingleControlFlowInstruction : ControlFlowInstruction {

        public override int GetNumArguments() {
            return 1;
        }
        public override List<Type> GetArgCompatibilityAtPos(int pos) {
            return new List<Type> { typeof(ConditionalInstruction) };
        }

    }
}
