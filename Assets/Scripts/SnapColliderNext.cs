using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderNext : SnapCollider {
        public override Vector3 SnapPosition { get { return Vector3.down; } }

        public override HashSet<Type> CompatibleArgTypes { get; }
            = new HashSet<Type> { typeof(StandAloneInstruction) };
       
        protected override void RegisterToSnapColliderGroup() {
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                CommonSCKeys.Next,
                this);
        }
    }
}

