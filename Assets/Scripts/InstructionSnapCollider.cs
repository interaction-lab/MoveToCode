using System;
using System.Collections.Generic;

namespace MoveToCode {
    public abstract class InstructionSnapCollider : SnapCollider {
        protected override List<Type> GetMyCompatibleArgTypes() {
            if (myCompatibleArgTypes == null) {
                myCompatibleArgTypes = new List<Type> { typeof(StandAloneInstruction) };
            }
            return myCompatibleArgTypes;
        }
    }
}
