using System;
using System.Collections.Generic;

namespace MoveToCode {
    public class SnapColliderValue : SnapCollider {
        public override HashSet<Type> CompatibleArgTypes { get; } =
            new HashSet<Type> { typeof(IDataType),
                                typeof(MathInstruction),
                                typeof(ConditionalInstruction),
                                typeof(ArrayIndexInstruction) };
        protected override void RegisterToSnapColliderGroup() {
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                new KeyValuePair<Type, int>(
                    typeof(SnapColliderValue),
                    0),
                this);
        }
    }
}

