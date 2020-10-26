using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class SnapColliderLeftNumber : SnapCollider {
        public override HashSet<Type> CompatibleArgTypes { get; } = new HashSet<Type> {
            typeof(INumberDataType),
            typeof(MathInstruction) };

        protected override void RegisterToSnapColliderGroup() {
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(CommonSCKeys.LeftNumber, this);
        }
    }
}