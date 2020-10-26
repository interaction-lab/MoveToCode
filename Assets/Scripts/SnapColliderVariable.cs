using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class SnapColliderVariable : SnapCollider {
        public override HashSet<Type> CompatibleArgTypes { get; } =
            new HashSet<Type> { typeof(Variable),
                typeof(ArrayIndexInstruction) };

        protected override void RegisterToSnapColliderGroup() {
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                new KeyValuePair<Type, int>(
                    typeof(SnapColliderVariable),
                    0),
                this);
        }
    }
}

