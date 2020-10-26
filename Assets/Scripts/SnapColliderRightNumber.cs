using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderRightNumber : SnapCollider {
        public override HashSet<Type> CompatibleArgTypes { get; } =
            new HashSet<Type> { typeof(INumberDataType),
                                typeof(MathInstruction) };
    }
}

