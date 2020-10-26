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
                new KeyValuePair<Type, int>(
                    typeof(SnapColliderLeftNumber),
                    0),
                this);
        }
    }
}

