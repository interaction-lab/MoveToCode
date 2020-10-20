using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderRightOfConditional : SnapCollider {
        private HashSet<Type> _compatibleTypes = 
            new HashSet<Type> { typeof(BasicDataType),
                                typeof(MathInstruction),
                                typeof(ArrayIndexInstruction) };
        public HashSet<Type> compatibleTypes {
            get { return _compatibleTypes; }
            set { _compatibleTypes = value; }
        }
    }
}

