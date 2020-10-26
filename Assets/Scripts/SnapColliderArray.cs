using System;
using System.Collections.Generic;

namespace MoveToCode{
    public class SnapColliderArray : SnapCollider{
        public override HashSet<Type> CompatibleArgTypes { get; } = new HashSet<Type> {
            typeof(Variable) };

        protected override void RegisterToSnapColliderGroup(){
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                new KeyValuePair<Type, int>(
                    typeof(SnapColliderArray),
                    0),
                this);
        }
    }
}