using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class SnapColliderArrayElement : SnapCollider {
        public int index;

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
