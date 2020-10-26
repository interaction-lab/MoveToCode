using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderPrintable : SnapCollider {
        public override Vector3 SnapPosition { get;} = new Vector3(0, 0, -0.1f);
        public override HashSet<Type> CompatibleArgTypes { get; } =
            new HashSet<Type> { typeof(IDataType),
                                typeof(MathInstruction),
                                typeof(ConditionalInstruction),
                                typeof(ArrayIndexInstruction) };
        protected override void RegisterToSnapColliderGroup() {
            MyCodeBlock.GetSnapColliderGroup().RegisterSnapCollider(
                new KeyValuePair<Type, int>(
                    typeof(SnapColliderPrintable),
                    0),
                this);
        }
    }
}

