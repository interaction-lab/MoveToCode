﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderNested : SnapCollider {
        public override HashSet<Type> CompatibleArgTypes { get; }
            = new HashSet<Type> { typeof(StandAloneInstruction) };
    }
}

