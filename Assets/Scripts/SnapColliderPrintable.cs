using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderPrintable : SnapCollider {
        private HashSet<Type> _compatibleTypes = 
            new HashSet<Type> { typeof(IDataType),
                                typeof(MathInstruction),
                                typeof(ConditionalInstruction),
                                typeof(ArrayIndexInstruction) };
        public HashSet<Type> compatibleTypes {
            get { return _compatibleTypes; }
            set { _compatibleTypes = value; }
        }
    }
}

