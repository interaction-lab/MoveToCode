using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ArraySnapCollider : SnapCollider {
        public override Vector3 SnapPosition { get; } = Vector3.zero;
        public override HashSet<Type> CompatibleArgTypes { get; } = new HashSet<Type> {
            typeof(Variable) };

        protected override void RegisterToSnapColliderGroup() {
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                new KeyValuePair<Type, int>(
                    typeof(ArraySnapCollider),
                    0),
                this);
        }
    }
}