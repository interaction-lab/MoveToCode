using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderVariable : SnapCollider {
        private HashSet<Type> _compatibleTypes = 
            new HashSet<Type> { typeof(Variable), typeof(ArrayIndexInstruction) };
        public HashSet<Type> compatibleTypes {
            get { return _compatibleTypes; }
            set { _compatibleTypes = value; }
        }
    }
}

