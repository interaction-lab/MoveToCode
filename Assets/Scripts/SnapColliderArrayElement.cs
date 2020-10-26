using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderArrayElement : SnapCollider {
        public int index;

        public override Vector3 SnapPosition { get; } = new Vector3(0, 0, -0.1f);

        public override HashSet<Type> CompatibleArgTypes { get; } = new HashSet<Type> {
            typeof(BasicDataType) };

        protected override void RegisterToSnapColliderGroup(){
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                new KeyValuePair<Type, int>(
                    typeof(SnapColliderArrayElement),
                    index),
                this);
        }
    }
}
