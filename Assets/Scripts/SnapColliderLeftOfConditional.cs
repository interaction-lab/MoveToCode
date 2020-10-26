using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class SnapColliderLeftOfConditional : SnapCollider {
        public override HashSet<Type> CompatibleArgTypes { get; } =
            new HashSet<Type> { typeof(BasicDataType),
                                typeof(MathInstruction),
                                typeof(ArrayIndexInstruction) };

        protected override void RegisterToSnapColliderGroup() {
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                CommonSCKeys.LeftConditional,
                this);
        }
    }
}

