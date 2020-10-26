using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderVariable : SnapCollider {
        public override HashSet<Type> CompatibleArgTypes { get; } =
            new HashSet<Type> { typeof(Variable),
                typeof(ArrayIndexInstruction) };
    }
}

