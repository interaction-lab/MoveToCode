using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ArrayElementSnapCollider : SnapCollider {
        public int index;

        public void SetIndex(int index_in){
            index = index_in;
            RegisterToSnapColliderGroup(); // need to redo as this changes, really should be a pointer as second are in keyvalue
        }

        public override Vector3 SnapPosition { get; } = new Vector3(0, 0, -0.1f);

        public override HashSet<Type> CompatibleArgTypes { get; } = new HashSet<Type> {
            typeof(BasicDataType) };

        protected override void RegisterToSnapColliderGroup(){
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                new KeyValuePair<Type, int>(
                    typeof(ArrayElementSnapCollider),
                    index),
                this);
        }
    }
}
