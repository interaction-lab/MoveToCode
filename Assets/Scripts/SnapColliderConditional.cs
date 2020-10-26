using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class SnapColliderConditional : SnapCollider {
        public override HashSet<Type> CompatibleArgTypes { get; } = new HashSet<Type> {
            typeof(ConditionalInstruction) };

        protected override void RegisterToSnapColliderGroup(){
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                new KeyValuePair<Type, int>(
                    typeof(SnapColliderConditional),
                    0),
                this);
        }
    }
}

