using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderValue : SnapCollider {
        public override HashSet<Type> CompatibleArgTypes { get; } =
            new HashSet<Type> { typeof(IDataType),
                                typeof(MathInstruction),
                                typeof(ConditionalInstruction),
                                typeof(ArrayIndexInstruction) };
    }
}

