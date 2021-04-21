using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderNested : SnapCollider {
        public override Vector3 SnapPosition { get; } = new Vector3(0.2f, -1.0f, 0f);

        public override HashSet<Type> CompatibleArgTypes { get; }
            = new HashSet<Type> { typeof(SnappableStandAloneInstruction) };

        protected override void RegisterToSnapColliderGroup() {
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                CommonSCKeys.Nested,
                this);
        }
    }
}

