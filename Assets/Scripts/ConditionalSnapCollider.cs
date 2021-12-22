using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ConditionalSnapCollider : SnapCollider {
        public override Vector3 SnapPosition { get; } = new Vector3(0, 0, -0.1f);
        public override HashSet<Type> CompatibleArgTypes { get; } = new HashSet<Type> {
            typeof(ConditionalInstruction) };

        protected override void RegisterToSnapColliderGroup(){
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                CommonSCKeys.Conditional,
                this);
        }
    }
}

