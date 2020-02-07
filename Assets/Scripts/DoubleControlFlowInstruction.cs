using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class DoubleControlFlowInstruction : ControlFlowInstruction {
        public override int GetNumArguments() {
            return 2;
        }

        public override List<Type> GetArgCompatibilityAtPos(int pos) {
            if (pos == 0) {
                return new List<Type> { typeof(ConditionalInstruction) };
            }
            else {
                return new List<Type> { typeof(StandAloneInstruction) }; // TODO: this is hack for else instruction
            }
        }
    }
}



