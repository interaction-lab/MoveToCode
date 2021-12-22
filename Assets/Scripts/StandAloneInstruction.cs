using System.Collections.Generic;

namespace MoveToCode {
    public abstract class StandAloneInstruction : Instruction {

        public StandAloneInstruction(CodeBlock cbIn) : base(cbIn) { }

        public virtual StandAloneInstruction GetNextInstruction() {
            return GetArgument(new KeyValuePair<System.Type, int>(typeof(NextSnapCollider), 0)) as StandAloneInstruction;
        }
    }
}
