using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class SnapColliderNext : SnapCollider {
        public override HashSet<Type> CompatibleArgTypes { get; }
            = new HashSet<Type> { typeof(StandAloneInstruction) };

        protected override void RegisterToSnapColliderGroup() {
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                new KeyValuePair<Type, int>(
                    typeof(SnapColliderNext),
                    0),
                this);
        }
    }
}

