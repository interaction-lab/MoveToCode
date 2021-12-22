using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class NestedSnapCollider : SnapCollider {
        public override Vector3 SnapPosition { 
            get{
            return new Vector3(0.15f, transform.localPosition.y, 0);
        } }

        public override HashSet<Type> CompatibleArgTypes { get; }
            = new HashSet<Type> { typeof(SnappableStandAloneInstruction) };

        protected override void RegisterToSnapColliderGroup() {
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                CommonSCKeys.Nested,
                this);
        }
    }
}

